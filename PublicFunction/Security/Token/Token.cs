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

            public interface IJWTAlgorithm
            {
                JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime);
                T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure;
            }

            public class HS256 : IJWTAlgorithm
            {
                private string _secret;

                public HS256(string secret)
                {
                    _secret = secret;
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA256Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(_secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(_secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class HS384 : IJWTAlgorithm
            {
                private string _secret;

                public HS384(string secret)
                {
                    _secret = secret;
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA384Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(_secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(_secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class HS512 : IJWTAlgorithm
            {
                private string _secret;

                public HS512(string secret)
                {
                    _secret = secret;
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA512Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(_secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(_secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class RS256 : IJWTAlgorithm
            {
                private RSA _rsa = new RSACryptoServiceProvider(1024);
                private RSAParameters _privateKey;
                private RSAParameters _publicKey;

                public RSAParameters PrivateKey
                {
                    get => _privateKey;
                    set => _privateKey = value;
                }

                public RSAParameters PublicKey
                {
                    get => _publicKey;
                    set => _publicKey = value;
                }

                public string PrivateKey_XML
                {
                    get => _rsa.ToXmlString(true);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _privateKey = rsa.ExportParameters(true);
                    }
                }

                public string PublicKey_XML
                {
                    get => _rsa.ToXmlString(false);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _publicKey = rsa.ExportParameters(false);
                    }
                }

                public RS256()
                {
                    _privateKey = _rsa.ExportParameters(true);
                    _publicKey = _rsa.ExportParameters(false);
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var privateKeyRsa = RSA.Create(_privateKey);
                    var publicKeyRsa = RSA.Create(_publicKey);
                    var algorithm = new RS256Algorithm(publicKeyRsa, privateKeyRsa);
                    return JwtBuilder.Create().WithAlgorithm(algorithm)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithAlgorithm(new RS256Algorithm(RSA.Create(_publicKey), RSA.Create(_privateKey)))
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class RS384 : IJWTAlgorithm
            {
                private RSA _rsa = new RSACryptoServiceProvider(1024);
                private RSAParameters _privateKey;
                private RSAParameters _publicKey;

                public RSAParameters PrivateKey
                {
                    get => _privateKey;
                    set => _privateKey = value;
                }

                public RSAParameters PublicKey
                {
                    get => _publicKey;
                    set => _publicKey = value;
                }

                public string PrivateKey_XML
                {
                    get => _rsa.ToXmlString(true);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _privateKey = rsa.ExportParameters(true);
                    }
                }

                public string PublicKey_XML
                {
                    get => _rsa.ToXmlString(false);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _publicKey = rsa.ExportParameters(false);
                    }
                }

                public RS384()
                {
                    _privateKey = _rsa.ExportParameters(true);
                    _publicKey = _rsa.ExportParameters(false);
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var privateKeyRsa = RSA.Create(_privateKey);
                    var publicKeyRsa = RSA.Create(_publicKey);
                    var algorithm = new RS384Algorithm(publicKeyRsa, privateKeyRsa);
                    return JwtBuilder.Create().WithAlgorithm(algorithm)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithAlgorithm(new RS384Algorithm(RSA.Create(_publicKey), RSA.Create(_privateKey)))
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class RS512 : IJWTAlgorithm
            {
                private RSA _rsa = new RSACryptoServiceProvider(1024);
                private RSAParameters _privateKey;
                private RSAParameters _publicKey;

                public RSAParameters PrivateKey
                {
                    get => _privateKey;
                    set => _privateKey = value;
                }

                public RSAParameters PublicKey
                {
                    get => _publicKey;
                    set => _publicKey = value;
                }

                public string PrivateKey_XML
                {
                    get => _rsa.ToXmlString(true);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _privateKey = rsa.ExportParameters(true);
                    }
                }

                public string PublicKey_XML
                {
                    get => _rsa.ToXmlString(false);
                    set
                    {
                        var rsa = new RSACryptoServiceProvider();
                        rsa.FromXmlString(value);
                        _publicKey = rsa.ExportParameters(false);
                    }
                }

                public RS512()
                {
                    _privateKey = _rsa.ExportParameters(true);
                    _publicKey = _rsa.ExportParameters(false);
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var privateKeyRsa = RSA.Create(_privateKey);
                    var publicKeyRsa = RSA.Create(_publicKey);
                    var algorithm = new RS512Algorithm(publicKeyRsa, privateKeyRsa);
                    return JwtBuilder.Create().WithAlgorithm(algorithm)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now);

                }
            }
        }
    }
}
