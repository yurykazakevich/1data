using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Borigran.OneData.Authorization
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "GfhjdjpbrNfhfynfc_2022+";   // ключ для шифрации
        public const int AUTHTOKENEXPIRED = 2;
        public const int REFRESHTOKENEXPIRED = 50;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
