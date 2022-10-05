using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using NHibernate.Criterion;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Borigran.OneData.Authorization.Domain.Entities;
using Borigran.OneData.Platform.Encryption;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.Platform.Helpers;
using NHibernate.Linq;

namespace Borigran.OneData.Authorization.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> userRepository;
        private readonly ISmsSender smsSender;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IHashEncryptor encryptor;
        private readonly AuthOptions authOptions;
        private readonly IPhoneNumberHelper phoneNumberHelper;

        public AuthService(IRepository<User> userRepository, 
            ITokenGenerator tokenGenerator, 
            ISmsSender smsSender,
            IHashEncryptor encryptor,
            AuthOptions authOptions,
            IPhoneNumberHelper phoneNumberHelper)
        {
            this.userRepository = userRepository;
            this.tokenGenerator = tokenGenerator;
            this.smsSender = smsSender;
            this.encryptor = encryptor;
            this.authOptions = authOptions;
            this.phoneNumberHelper = phoneNumberHelper;
        }

        public async Task<string> GetVerificationCodeAsync(string phoneNumber)
        {
            string cleanedPhoneNumber = phoneNumberHelper.ClearPhoneNumber(phoneNumber);

            int code = new Random().Next(111111,999999);
#if DEBUG
            code = 123456;
#endif
            var smsResponse = await smsSender.SendAuthCodeAsync(cleanedPhoneNumber, code);

            //TODO: Add response validation

            string encriptedCode = encryptor.GetHash(SaltPhoneWithCode(phoneNumber, code.ToString()));

            return encriptedCode;
        }

        [Transaction]
        public async Task<AuthTokenDto> RegisterOrLoginAsync(LoginDto data)
        {
            string cleanedPhoneNumber = phoneNumberHelper.ClearPhoneNumber(data.PhoneNumber);

            if (!ValdateCode(cleanedPhoneNumber, data.VerificationCode, data.UserProvidedCode))
            {
                throw new SecurityTokenException("Invalidverification code");
            }

            var user = await FindUserAsync(cleanedPhoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumberHelper.ClearPhoneNumber(cleanedPhoneNumber),
                    IsPhisical = data.IsPhisical
                };
            }
            return await GenerateTokensForUser(user);
        }

        [Transaction]
        public async Task LogoutAsync(string phoneNumber)
        {
            var user = await FindUserAsync(phoneNumber);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpired = null;

                await userRepository.UpdateAsync(user);
            }
        }

        [Transaction]
        public async Task<AuthTokenDto> RefreshExpiredTokenAsync(string expiredToken, string refreshToken, string phoneNumber)
        {
            var principal = tokenGenerator.GetPrincipalFromToken(expiredToken, true);
            if (principal.Identity != null && principal.Identity.Name != phoneNumber)
            {
                throw new SecurityTokenException("Invalid user");
            }

            var user = await FindUserAsync(phoneNumber);
            if (user == null)
            {
                throw new SecurityTokenException("User not found");
            }

            if (user.RefreshToken != refreshToken ||
                user.RefreshTokenExpired < DateTime.Now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return await GenerateTokensForUser(user);
        }

        private async Task<AuthTokenDto> GenerateTokensForUser(User user)
        {
            DateTime tokenGeneratedTime = DateTime.UtcNow;
            SetRefreshTokenForUser(user, tokenGeneratedTime);

            user = await userRepository.SaveOrUpdateAsync(user);

            return new AuthTokenDto
            {
                Token = tokenGenerator.GenerateAccessTokenForUser(user, tokenGeneratedTime),
                RefreshToken = user.RefreshToken,
                TokenExpired = tokenGeneratedTime.AddMinutes(authOptions.AuthTokenExpired),
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber
            };
        }

        private void SetRefreshTokenForUser(User user, DateTime now)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpired = now.ToLocalTime()
                .AddMinutes(authOptions.RefreshTokenExpired);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private bool ValdateCode(string phoneNumber, string verificationCode, string userProvidedCode)
        {
            return encryptor.ValidateHash(verificationCode, SaltPhoneWithCode(phoneNumber, userProvidedCode));
        }

        private async Task<User> FindUserAsync(string phoneNumber)
        {
            return await userRepository.Query()
                .Where(x => x.PhoneNumber == phoneNumber)
                .FirstOrDefaultAsync();
        }

        private string SaltPhoneWithCode(string phoneNumber, string code)
        {
            const string salt = "Gfhjdjpbr";

            return $"{phoneNumber}_{salt}_{code}";
        }
    }
}
