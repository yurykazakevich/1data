using Borigran.OneData.Authorization.Domain.Entities;
using System.Security.Claims;

namespace Borigran.OneData.Authorization
{
    public interface IAuthService
    {
        string GetVerificationCode(string phoneNumber);
        User FindUser(string phoneNumber);
        User RegisterOrLogin(string phoneNumber, string verificationCode, int userProvidedCode);
        void Logout(string phoneNumber);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string RefreshExpiredToken(string expiredToken, string refreshToken, string phoneNumber);
    }
}