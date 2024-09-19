using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using JWT;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Security
{
    public class Token
    {
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
            public interface IRS256
            {
                public RSAParameters PrivateKey { get; set; }
                public RSAParameters PublicKey { get; set; }

                public string PrivateKey_XML { get; set; }
                public string PublicKey_XML { get; set; }
                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime);
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSAParameters PublicKey) where T : TokenBaseStructure;
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, string PublicKeyXML) where T : TokenBaseStructure;
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey, RSA PublicKey) where T : TokenBaseStructure;
            }
            public class RS256 : IRS256
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
