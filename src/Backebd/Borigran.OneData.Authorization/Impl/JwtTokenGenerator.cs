using Borigran.OneData.Authorization.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Borigran.OneData.Authorization.Impl
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly AuthOptions authOptions;

        public JwtTokenGenerator(AuthOptions authOptions)
        {
            this.authOptions = authOptions;
        }
        public static TokenValidationParameters TokenValidationParameters(AuthOptions authOptions)
        {
            return new TokenValidationParameters
            {
                // указывает, будет ли валидироваться издатель при валидации токена
                ValidateIssuer = true,
                // строка, представляющая издателя
                ValidIssuer = authOptions.Issuer,
                // будет ли валидироваться потребитель токена
                ValidateAudience = true,
                // установка потребителя токена
                ValidAudience = authOptions.Audience,
                // будет ли валидироваться время существования
                ValidateLifetime = true,
                // установка ключа безопасности
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                // валидация ключа безопасности
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true
            };
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool isExpired = false)
        {
            var tokenValidationParameters = TokenValidationParameters(authOptions);
            tokenValidationParameters.ValidateLifetime = !isExpired;

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GenerateAccessTokenForUser(User user)
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
                    expires: DateTime.UtcNow.AddMinutes(authOptions.AuthTokenExpired),
                    signingCredentials:
                        new SigningCredentials(signinCredentials, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
