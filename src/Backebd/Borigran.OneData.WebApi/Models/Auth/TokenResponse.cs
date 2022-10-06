using System;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpired { get; set; }
        public int Userid { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhisical { get; set; }
    }
}
