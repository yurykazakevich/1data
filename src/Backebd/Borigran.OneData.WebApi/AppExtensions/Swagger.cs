using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Borigran.OneData.WebApi.AppExtensions
{
    public static class Swagger
    {

        public static IServiceCollection AddOneDataSwaggerGen(this IServiceCollection services)
        {
            return services.AddSwaggerGen(SetUpJwtSwaggerGenOptions);
        }
        
        public static IApplicationBuilder UseOneDataSwagger(this IApplicationBuilder app)
        {
            IApplicationBuilder oneData = app.UseSwagger();
            return oneData.UseSwaggerUI(GetSwaggerOptions());
        }

        private static SwaggerUIOptions GetSwaggerOptions()
        {
            var options = new SwaggerUIOptions
            {
                RoutePrefix = string.Empty,
            };

            options.SwaggerEndpoint("/swagger/v1/swagger.json", "OneData v1");

            return options;
        }

        private static void SetUpJwtSwaggerGenOptions(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "OneData API",
                Version = "v1"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.\r\n\r\nEnter 'Bearer' [space] and then your token in input below.\r\n\r\nExample: \"Bearer jdhfjkdsgjhjkshgsjk\""
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {

                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
        }
    }
}
