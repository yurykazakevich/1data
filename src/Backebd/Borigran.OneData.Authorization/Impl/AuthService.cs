using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using NHibernate.Criterion;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Borigran.OneData.Authorization.Domain.Entities;
using Borigran.OneData.Platform.Encryption;
using Borigran.OneData.Authorization.Dto;
using Borigran.OneData.Platform.Helpers;

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
            int code = new Random().Next(111111,999999);
#if DEBUG
            code = 123456;
#endif
            var smsResponse = await smsSender.SendAuthCodeAsync(phoneNumber, code);

            //TODO: Add response validation

            string encriptedCode = encryptor.GetHash(code.ToString());

            return encriptedCode;
        }

        [Transaction]
        public async Task<AuthTokenDto> RegisterOrLoginAsync(string phoneNumber, string verificationCode, string userProvidedCode)
        {
            string cleanedPhoneNumber = phoneNumberHelper.ClearPhoneNumber(phoneNumber);

            if (!ValdateCode(verificationCode, userProvidedCode))
            {
                throw new SecurityTokenException("Invalidverification code");
            }

            var user = await FindUserAsync(cleanedPhoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumberHelper.ClearPhoneNumber(cleanedPhoneNumber),
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

            await userRepository.UpdateAsync(user);

            return new AuthTokenDto
            {
                Token = tokenGenerator.GenerateAccessTokenForUser(user, tokenGeneratedTime),
                RefreshToken = user.RefreshToken,
                TokenCreated = tokenGeneratedTime,
                TokenExpired = authOptions.AuthTokenExpired,
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
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private bool ValdateCode(string verificationCode, string userProvidedCode)
        {
            return encryptor.ValidateHash(verificationCode, userProvidedCode);
        }

        private async Task<User> FindUserAsync(string phoneNumber)
        {
            return await userRepository.FindOneAsync(Restrictions.Where<User>(x => x.PhoneNumber == phoneNumber));
        }
    }
}
