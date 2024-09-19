
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using JWT;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace PublicFunction;
public class Security
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

    public class Token {
        public class JWTToken
        {
            public class TokenBaseStructure
            {
                public string uid { get; set; }
                public string iss { get; set; }
                public string aud { get; set; }
                public double iat { get; set; }
                public double nbf { get; set; }
                public double exp { get; set; }
            }
            public interface IRS256 {
                public RSAParameters PrivateKey { get; set; }
                public RSAParameters PublicKey { get; set; }

                public string PrivateKey_XML { get; set; }
                public string PublicKey_XML { get; set; }
                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime);
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSAParameters PublicKey) where T : TokenBaseStructure;
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, string PublicKeyXML) where T : TokenBaseStructure;
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSA PublicKey) where T : TokenBaseStructure;
            }
            public class RS256:IRS256
            {
                RSA _RSA = new RSACryptoServiceProvider(1024);
                private RSAParameters _PrivateKey;

                private RSAParameters _PublicKey;

                public RSAParameters PrivateKey
                {
                    get
                    {
                        return _PrivateKey;
                    }
                    set
                    {
                        _PrivateKey = value;
                    }
                }

                public RSAParameters PublicKey
                {
                    get
                    {
                        return _PublicKey;
                    }
                    set
                    {
                        _PublicKey = value;
                    }
                }

                public string PrivateKey_XML
                {
                    get
                    {
                        return _RSA.ToXmlString(includePrivateParameters: true);
                        //return PrivateKey_RSA.ToXmlString(includePrivateParameters: true);
                    }
                    set
                    {

                        RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                        rSACryptoServiceProvider.FromXmlString(value);
                        _PrivateKey = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
                    }
                }

                public string PublicKey_XML
                {
                    get
                    {
                        return _RSA.ToXmlString(includePrivateParameters: false);
                        //return PublicKey_RSA.ToXmlString(includePrivateParameters: false);
                    }
                    set
                    {
                        RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                        rSACryptoServiceProvider.FromXmlString(value);
                        _PublicKey = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
                    }
                }

                public RS256()
                {
                    PrivateKey = _RSA.ExportParameters(includePrivateParameters: true);
                    PublicKey = _RSA.ExportParameters(includePrivateParameters: false);
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    RSA PrivateKey_RSA = RSA.Create(PrivateKey), PublicKey_RSA = RSA.Create(PublicKey);
                    IJwtAlgorithm algorithm = new RS256Algorithm(PublicKey_RSA, PrivateKey_RSA);
                    return JwtBuilder.Create().WithAlgorithm(algorithm).Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSAParameters PublicKey) where T : TokenBaseStructure
                {
                    RSA publicKey = RSA.Create(PublicKey);
                    return Decrypt<T>(JwtToken, Issuer, Audience, ValidateLifetime, ValidateIssuerSigningKey, publicKey);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, string PublicKeyXML) where T : TokenBaseStructure
                {
                    RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
                    rSACryptoServiceProvider.FromXmlString(PublicKeyXML);
                    return Decrypt<T>(JwtToken, Issuer, Audience, ValidateLifetime, ValidateIssuerSigningKey, rSACryptoServiceProvider);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSA PublicKey) where T : TokenBaseStructure
                {
                    try
                    {
                        JwtToken = JwtToken.Replace("Bearer ", string.Empty);
                        IJsonSerializer jsonSerializer = new JsonNetSerializer();
                        IDateTimeProvider dateTimeProvider = new UtcDateTimeProvider();
                        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                        TokenValidationParameters validationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = !(Issuer == ""),
                            ValidateAudience = !(Audience == ""),
                            ValidateLifetime = ValidateLifetime,
                            ValidIssuer = Issuer,
                            ValidAudience = Audience,
                            ValidateIssuerSigningKey = ValidateIssuerSigningKey,
                            IssuerSigningKey = new RsaSecurityKey(PublicKey)
                        };
                        if (!jwtSecurityTokenHandler.CanReadToken(JwtToken))
                        {
                            throw new Exception("Can't Read Token");
                        }

                        jwtSecurityTokenHandler.ValidateToken(JwtToken, validationParameters, out var _);
                        string json = new JwtDecoder(jsonSerializer, new JwtValidator(jsonSerializer, dateTimeProvider), new JwtBase64UrlEncoder(), new HMACSHA256Algorithm()).Decode(JwtToken, verify: false);
                        return System.Text.Json.JsonSerializer.Deserialize<T>(json); 
                        //Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                

            }
            
            
        }
    }
}