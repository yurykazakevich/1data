using Borigran.OneData.WebApi.Pipeline.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace Borigran.OneData.WebApi.Pipeline.RefreshToken
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate next;

        public SecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await next.Invoke(context);

            /*context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            string refreshToken = RefreshTokenCookieManager.GetToken(context.Request);

            if (!string.IsNullOrEmpty(refreshToken))
            {
                RefreshTokenCookieManager.AddCookie(context.Response, refreshToken);
            }*/
        }
    }
}
