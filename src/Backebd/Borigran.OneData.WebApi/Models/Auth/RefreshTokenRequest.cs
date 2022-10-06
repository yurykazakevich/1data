namespace Borigran.OneData.WebApi.Models.Auth
{
    public class RefreshTokenRequest : UserIdRequest
    {
        public string ExpiredToken { get; set; }
    }
}
