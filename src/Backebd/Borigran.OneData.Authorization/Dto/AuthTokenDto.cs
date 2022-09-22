
namespace Borigran.OneData.Authorization.Dto
{
    public class AuthTokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int TokenExpired { get; set; }
        public DateTime TokenCreated { get; set; }
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
