namespace Borigran.OneData.WebApi.Models.Auth
{
    public class LoginRequest : PhoneNumberRequest
    {
        public string VerificationCode { get; set; }
        public string UserProvidedCode { get; set; }
        public bool IsPhisical { get; set; }
    }
}
