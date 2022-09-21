using System.Text.RegularExpressions;

namespace Borigran.OneData.Platform.Helpers
{
    public class PhoneNumberHelper : IPhoneNumberHelper
    {
        public Regex CleanPhoneNumberRegex { get; private set; } = new Regex(@"(\(|\)| |-)");

        public Regex ValidatePhoneNumberRegex { get; private set; } =
            new Regex(@"^\+\d{1,3}\({0,1}\d{2,3}\){0,1}\d{7}$");

        public string ClearPhoneNumber(string phoneNumber)
        {
            return CleanPhoneNumberRegex.Replace(phoneNumber, string.Empty);
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            return ValidatePhoneNumberRegex.IsMatch(phoneNumber);
        }
    }
}
