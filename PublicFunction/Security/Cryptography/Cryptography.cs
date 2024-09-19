using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Security
{
    public class Cryptography
    {
        public class RSAAlgorithm
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
            /// <summary>
            /// Decrypted Text
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
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
            /// <summary>
            /// Text to encrypt
            /// </summary>
            public string Encrypt(string data)
            {
                rsa.FromXmlString(_PublicKey);
                var dataToEncrypt = _encoder.GetBytes(data);
                var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
                var length = encryptedByteArray.Count();
                var item = 0;
                var sb = new StringBuilder();
                foreach (var x in encryptedByteArray)
                {
                    item++;
                    sb.Append(x);

                    if (item < length)
                        sb.Append(",");
                }

                return sb.ToString();
            }
        }
        public class AesAlgorithm
        {
            byte[] salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
            //new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }
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
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }

            public string Decrypt(string cipherText, string Key = "MAKV2SPBNI99212")
            {
                //byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, salt);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    //let iv = CryptoJS.lib.WordArray.create(key.words.slice(0, 4));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }



        }
    }
}
