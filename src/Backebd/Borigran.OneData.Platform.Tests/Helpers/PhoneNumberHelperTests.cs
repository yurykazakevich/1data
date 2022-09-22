using Borigran.OneData.Platform.Helpers;

namespace Borigran.OneData.Platform.Tests.Helpers
{
    public class PhoneNumberHelperTests
    {
        private IPhoneNumberHelper sut;

        [SetUp]
        public void Setup()
        {
            sut = new PhoneNumberHelper();
        }

        [Test]
        public void CleanPhoneNumberTest()
        {
            string phoneNumber = "+375(29)855-84 38";
            string expectedPhoneNumber = "+375298558438";

            var cleanedPhoneNumber = sut.ClearPhoneNumber(phoneNumber);

            Assert.That(expectedPhoneNumber.Equals(cleanedPhoneNumber));
        }

        [Test]
        public void Validate_ValidBelorussianPhoneNumber_Test()
        {
            string phoneNumber = "+375(29)8558438";

            var isValid = sut.ValidatePhoneNumber(phoneNumber);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Validate_ValidRussianPhoneNumber_Test()
        {
            string phoneNumber = "+7(910)7945508";

            var isValid = sut.ValidatePhoneNumber(phoneNumber);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Validate_ShortPhoneNumber_Test()
        {
            string phoneNumber = "+7(910)794558";

            var isValid = sut.ValidatePhoneNumber(phoneNumber);

            Assert.IsFalse(isValid);
        }

        [Test]
        public void Validate_NoPlusPhoneNumber_Test()
        {
            string phoneNumber = "7(910)794558";

            var isValid = sut.ValidatePhoneNumber(phoneNumber);

            Assert.IsFalse(isValid);
        }

        [Test]
        public void Validate_InvalidSymboslPhoneNumber_Test()
        {
            string phoneNumber = "7(910)79455s8";

            var isValid = sut.ValidatePhoneNumber(phoneNumber);

            Assert.IsFalse(isValid);
        }
    }
}
