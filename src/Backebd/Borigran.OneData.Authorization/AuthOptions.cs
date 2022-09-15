using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Borigran.OneData.Authorization
{
    public class AuthOptions
    {
        const string KEY = "GfhjdjpbrNfhfynfc_2022+";   // ключ для шифрации
        public string Issuer { get; set; } // издатель токена
        public string Audience { get; set; } // потребитель токена
        public int AuthTokenExpired { get; set; }
        public int RefreshTokenExpired { get; set; }
        public string SendSmsEndpoint { get; set; }
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
