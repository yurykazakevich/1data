using Borigran.OneData.Domain.Entities;
using Borigran.OneData.Platform.NHibernate.Repository;
using Borigran.OneData.Platform.NHibernate.Transactions;
using NHibernate.Criterion;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Borigran.OneData.Authorization.Domain.Entities;

namespace Borigran.OneData.Authorization.Jwt
{
    public class AuthService : IAuthService, ITransactionContainer
    {
        private readonly IRepository<User> userRepository;

        public AuthService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public User FindUser(string phoneNumber)
        {
            return userRepository.FindOne(Restrictions.Where<User>(x => x.PhoneNumber == phoneNumber));
        }

        [Transaction]
        public User RegisterOrLogin(string phoneNumber)
        {
            var user = FindUser(phoneNumber);

            if (user == null)
            {
                user = new User
                {
                    PhoneNumber = phoneNumber,
                };
            }

            UpdateRefreshToken(user);

            userRepository.SaveOrUpdate(user);
            return user;
        }

        [Transaction]
        public void Logout(string phoneNumber)
        {
            var user = FindUser(phoneNumber);

            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpired = null;

                userRepository.Update(user);
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {

            var signinCredentials = AuthOptions.GetSymmetricSecurityKey();

            var tokeOptions = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(AuthOptions.AUTHTOKENEXPIRED),
                    signingCredentials:
                        new SigningCredentials(signinCredentials, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        [Transaction]
        public string RefreshExpiredToken(string expiredToken, string refreshToken, string phoneNumber)
        {
            var principal = GetPrincipalFromExpiredToken(expiredToken);
            if (principal.Identity != null && principal.Identity.Name != phoneNumber)
            {
                throw new SecurityTokenException("Invalid user");
            }

            var user = FindUser(phoneNumber);
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

            userRepository.Update(user);

            return user.RefreshToken;
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
            user.RefreshTokenExpired = DateTime.Now.AddMinutes(AuthOptions.REFRESHTOKENEXPIRED);
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
    }
}
