using System.ComponentModel;

namespace Borigran.OneData.WebApi.Models.Auth
{
    public class LoginRequest : PhoneNumberRequest
    {
        [DefaultValue("/TzCVPodeY3mx9zHHe5QbxL4Ag0=")]
        public string VerificationCode { get; set; }

        [DefaultValue("123456")]
        public string UserProvidedCode { get; set; }
    }
}
