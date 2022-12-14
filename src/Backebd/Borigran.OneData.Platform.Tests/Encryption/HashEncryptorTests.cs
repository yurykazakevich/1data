using Borigran.OneData.Platform.Encryption;

namespace Borigran.OneData.Platform.Tests.Encryption
{
    public class HashEncryptorTests
    {
        private HashEncryptor sut;

        [SetUp]
        public void Setup()
        {
            sut = new HashEncryptor();
        }


        [Test]
        public void ValidateHash_MustBeValid()
        {
            string code = new Random().Next(100000, 999999).ToString();

            string hash = sut.GetHash(code);
            bool result = sut.ValidateHash(hash, code);

            Assert.IsTrue(result);
        }

        [Test]
        public void ValidateHash_MustBeInValid()
        {
            int code = new Random().Next(100000, 999999);

            string hash = sut.GetHash(code.ToString());
            code--;
            bool result = sut.ValidateHash(hash, code.ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public void ValidateHash_RussianChars_MustBeValid()
        {
            string data = "??????????";

            string hash = sut.GetHash(data);
            bool result = sut.ValidateHash(hash, data);

            Assert.IsTrue(result);
        }
    }
}