namespace Borigran.OneData.Platform.Encryption
{
    public interface IHashEncryptor
    {
        public string GetHash(string toEncrypt);

        public bool ValidateHash(string hash, string toValidate);
    }
}
