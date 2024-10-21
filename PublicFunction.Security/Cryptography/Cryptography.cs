using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace PublicFunction.Security.Cryptography
{
    public class CryptographyAlgorithm
    {
        public interface IRSACryptographyAlgorithm
        {
            string Encrypt(string data);
            string Decrypt(string data);
        }
        public interface IAesCryptographyAlgorithm
        {
            public string Encrypt(string clearText, string Key);
            public string Decrypt(string cipherText, string Key);
        }

        public class RSAAlgorithm : IRSACryptographyAlgorithm
        {
            private string _PrivateKey { get; set; }
            private string _PublicKey { get; set; }
            private UnicodeEncoding _encoder = new UnicodeEncoding();
            private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            public RSAAlgorithm()
            {
                _PrivateKey = rsa.ToXmlString(true);
                _PublicKey = rsa.ToXmlString(false);
            }

            public RSAAlgorithm(string PrivateKey, string PublicKey)
            {
                _PrivateKey = PrivateKey;
                _PublicKey = PublicKey;
            }

            public RSAAlgorithm(string PublicKey)
            {
                _PublicKey = PublicKey;
            }

            public string Encrypt(string data)
            {
                rsa.FromXmlString(_PublicKey);
                var dataToEncrypt = _encoder.GetBytes(data);
                var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
                var sb = new StringBuilder();
                for (int i = 0; i < encryptedByteArray.Length; i++)
                {
                    sb.Append(encryptedByteArray[i]);
                    if (i < encryptedByteArray.Length - 1)
                        sb.Append(",");
                }
                return sb.ToString();
            }

            public string Decrypt(string data)
            {
                var dataArray = data.Split(new char[] { ',' });
                byte[] dataByte = new byte[dataArray.Length];
                for (int i = 0; i < dataArray.Length; i++)
                {
                    dataByte[i] = Convert.ToByte(dataArray[i]);
                }
                rsa.FromXmlString(_PrivateKey);
                var decryptedByte = rsa.Decrypt(dataByte, false);
                return _encoder.GetString(decryptedByte);
            }
        }

        public class AesAlgorithm : IAesCryptographyAlgorithm
        {
            private static readonly byte[] salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

            public string Encrypt(string clearText, string Key = "MAKV2SPBNI99212")
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, salt);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            public string Decrypt(string cipherText, string Key = "MAKV2SPBNI99212")
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, salt);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        return Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
