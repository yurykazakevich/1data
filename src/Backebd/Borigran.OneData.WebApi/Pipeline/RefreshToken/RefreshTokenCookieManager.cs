using Microsoft.AspNetCore.Http;

namespace Borigran.OneData.WebApi.Pipeline.RefreshToken
{
    public class RefreshTokenCookieManager
    {
        private const string CookieKey = "Burial.Id";

        public static string? GetToken(HttpRequest request)
        {
            string? token;
            request.Cookies.TryGetValue(CookieKey, out token);

            return token;
        }

        public static void AddCookie(HttpResponse response, string token)
        {
            response.Cookies.Append(CookieKey, token);
        }

        public static void ClearToken(HttpResponse response)
        {
            response.Cookies.Delete(CookieKey);
        }
    }
}
