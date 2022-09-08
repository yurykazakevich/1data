using Borigran.OneData.Authorization.Domain.Entities;
using System.Security.Claims;

namespace Borigran.OneData.Authorization
{
    public interface IAuthService
    {
        User FindUser(string phoneNumber);
        User RegisterOrLogin(string phoneNumber);
        void Logout(string phoneNumber);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string RefreshExpiredToken(string expiredToken, string refreshToken, string phoneNumber);
    }
}