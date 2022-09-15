using Borigran.OneData.Authorization.Domain.Entities;
using System.Security.Claims;

namespace Borigran.OneData.Authorization
{
    public interface IAuthService
    {
        Task<string> GetVerificationCodeAsync(string phoneNumber);
        Task<User> FindUserAsync(string phoneNumber);
        Task<User> RegisterOrLoginAsync(string phoneNumber, string verificationCode, int userProvidedCode);
        Task LogoutAsync(string phoneNumber);
        string GenerateAccessToken(IEnumerable<Claim> claims);
        Task<string> RefreshExpiredTokenAsync(string expiredToken, string refreshToken, string phoneNumber);
    }
}