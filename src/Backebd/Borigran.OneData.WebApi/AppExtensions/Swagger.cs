using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Borigran.OneData.WebApi.AppExtensions
{
    public static class Swagger
    {
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

            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

            return options;
        }
    }
}
