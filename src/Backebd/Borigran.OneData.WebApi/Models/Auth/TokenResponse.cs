using System;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public int TokenExpired { get; set; }
        public DateTime TokenCreated { get; set; }
        public int Userid { get; set; }
        public string PhoneNumber { get; set; }
    }
}
