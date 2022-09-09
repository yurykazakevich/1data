using System;
using System.Security.Cryptography;
using System.Text;

namespace Borigran.OneData.Platform.Encryption
{
    public class HashEncryptor : IHashEncryptor
    {
        private const string hashKey = "FrdfkfyuNNfhfynfc";
        private readonly Encoding encoding = Encoding.UTF8;
        private readonly byte[] hashKeyBytes;

        public HashEncryptor()
        {
            hashKeyBytes = encoding.GetBytes(hashKey);
        }
        public string GetHash(string toEncrypt)
        {
            return GetHashString(toEncrypt);
        }

        public bool ValidateHash(string hash, string toValidate)
        {
            return hash.Equals(GetHashString(toValidate));
        }

        private string GetHashString(string data)
        {
            var byteData = encoding.GetBytes(data);
            var hash = GenerateHash(byteData);

            return Convert.ToBase64String(hash);
        }
        private byte[] GenerateHash(byte[] data)
        {
            using (var hmac = new HMACSHA1(hashKeyBytes))
            {
                return hmac.ComputeHash(data);
            }   
        }
    }
}
