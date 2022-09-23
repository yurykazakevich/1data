namespace Borigran.OneData.Authorization.Dto
{
    public class LoginDto
    {
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public string UserProvidedCode { get; set; }
        public bool IsPhisical { get; set; }
    }
}
