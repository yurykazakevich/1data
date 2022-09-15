using Borigran.OneData.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Borigran.OneData.WebApi.AppExtensions
{
    public static class Authentication
    {
        public static AuthenticationBuilder AddOneDataAuthentication(this IServiceCollection services,
            TokenValidationParameters tokenValidationParameters)
        {
            return services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
        }
    }
}
