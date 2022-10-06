using Borigran.OneData.Authorization.Dto;

namespace Borigran.OneData.Authorization
{
    public interface IAuthService
    {
        Task<string> GetVerificationCodeAsync(string phoneNumber);
        Task<AuthTokenDto> RegisterOrLoginAsync(LoginDto data);
        Task LogoutAsync(int userId);
        Task<AuthTokenDto> RefreshExpiredTokenAsync(string expiredToken, 
            string refreshToken, int userId);
    }
}