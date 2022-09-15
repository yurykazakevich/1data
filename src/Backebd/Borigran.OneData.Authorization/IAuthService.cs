using Borigran.OneData.Authorization.Dto;

namespace Borigran.OneData.Authorization
{
    public interface IAuthService
    {
        Task<string> GetVerificationCodeAsync(string phoneNumber);
        Task<AuthTokenDto> RegisterOrLoginAsync(string phoneNumber, string verificationCode, string userProvidedCode);
        Task LogoutAsync(string phoneNumber);
        Task<AuthTokenDto> RefreshExpiredTokenAsync(string expiredToken, string refreshToken, string phoneNumber);
    }
}