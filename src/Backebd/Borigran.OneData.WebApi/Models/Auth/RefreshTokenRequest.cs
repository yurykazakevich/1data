namespace Borigran.OneData.WebApi.Models.Auth
{
    public class RefreshTokenRequest : PhoneNumberRequest
    {
        public string ExpiredToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
