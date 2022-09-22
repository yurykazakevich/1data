using System.Text.RegularExpressions;

namespace Borigran.OneData.Platform.Helpers
{
    public interface IPhoneNumberHelper
    {
        public Regex CleanPhoneNumberRegex { get; }

        public Regex ValidatePhoneNumberRegex { get; }

        public string ClearPhoneNumber(string phoneNumber);

        public bool ValidatePhoneNumber(string phoneNumber);
    }
}
