﻿using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using NHibernate.Criterion;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Borigran.OneData.Authorization.Domain.Entities;
using Borigran.OneData.Platform.Encryption;
using Borigran.OneData.Authorization.Dto;

namespace Borigran.OneData.Authorization.Impl
{
    public class AuthService : IAuthService, ITransactionContainer
    {
        private readonly IRepository<User> userRepository;

        private readonly AuthOptions authOptions;

        private readonly ISmsSender smsSender;

        private readonly IHashEncryptor encryptor;

        public AuthService(IRepository<User> userRepository, 
            AuthOptions authOptions, 
            ISmsSender smsSender,
            IHashEncryptor encryptor)
        {
            this.userRepository = userRepository;
            this.authOptions = authOptions;
            this.smsSender = smsSender;
            this.encryptor = encryptor;
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
            if (!ValdateCode(verificationCode, userProvidedCode))
            {
                throw new SecurityTokenException("Invalidverification code");
            }

            var user = await FindUserAsync(phoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumber,
                };
            }

            UpdateRefreshToken(user);

            await userRepository.SaveOrUpdateAsync(user);

            return new AuthTokenDto
            {
                Token = GenerateAccessTokenForUser(user),
                RefreshToken = user.RefreshToken
            };
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
            var principal = GetPrincipalFromExpiredToken(expiredToken);
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

            UpdateRefreshToken(user);

            await userRepository.UpdateAsync(user);

            return new AuthTokenDto
            {
                Token = GenerateAccessTokenForUser(user),
                RefreshToken = user.RefreshToken
            };
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private void UpdateRefreshToken(User user)
        {
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpired = DateTime.Now.AddMinutes(authOptions.RefreshTokenExpired);
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

        private string GenerateAccessTokenForUser(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.PhoneNumber),
                new Claim(ClaimTypes.SerialNumber, user.Id.ToString())
            };

            return GenerateAccessToken(claims);

        }
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var signinCredentials = AuthOptions.GetSymmetricSecurityKey();

            var tokeOptions = new JwtSecurityToken(
                    issuer: authOptions.Issuer,
                    audience: authOptions.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(authOptions.AutheTokenExpired),
                    signingCredentials:
                        new SigningCredentials(signinCredentials, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
