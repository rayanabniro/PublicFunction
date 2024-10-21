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
using static PublicFunction.Security.Token.JWTToken;

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
            public interface IJWTAlgorithmHS256: IJWTAlgorithm
            {
                public string secret { get; set; }
            }
            public interface IJWTAlgorithmHS384 : IJWTAlgorithm
            {
                public string secret { get; set; }
            }
            public interface IJWTAlgorithmHS512 : IJWTAlgorithm
            {
                public string secret { get; set; }
            }
            public interface IJWTAlgorithmRS256 : IJWTAlgorithm
            {
                public RSAParameters PrivateKey { get; set; }
                public RSAParameters PublicKey { get; set; }
                public string PrivateKey_XML { get; set; }
                public string PublicKey_XML { get; set; }
            }
            public interface IJWTAlgorithmRS384 : IJWTAlgorithm
            {
                public RSAParameters PrivateKey { get; set; }
                public RSAParameters PublicKey { get; set; }
                public string PrivateKey_XML { get; set; }
                public string PublicKey_XML { get; set; }
            }
            public interface IJWTAlgorithmRS512 : IJWTAlgorithm
            {
                public RSAParameters PrivateKey { get; set; }
                public RSAParameters PublicKey { get; set; }
                public string PrivateKey_XML { get; set; }
                public string PublicKey_XML { get; set; }
            }
            public class HS256 : IJWTAlgorithmHS256
            {
                public string secret { get; set; }

                public HS256()
                {
                }

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA256Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class HS384 : IJWTAlgorithmHS384
            {
                public string secret { get; set; }

                

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA384Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class HS512 : IJWTAlgorithmHS512
            {
                public string secret { get; set; }

                

                public JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)
                {
                    var algorithm = new HMACSHA512Algorithm();
                    return JwtBuilder.Create().WithAlgorithm(algorithm).WithSecret(secret)
                        .Issuer(Issuer)
                        .IssuedAt(DateTime.Now)
                        .Audience(Audience)
                        .NotBefore(NotBefore)
                        .ExpirationTime(ExpirationTime);
                }

                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithSecret(secret)
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
            }

            public class RS256 : IJWTAlgorithmRS256
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

            public class RS384 : IJWTAlgorithmRS384
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

            public class RS512 : IJWTAlgorithmRS512
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
                public T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure
                {
                    var json = JwtBuilder.Create()
                        .WithAlgorithm(new RS384Algorithm(RSA.Create(_publicKey), RSA.Create(_privateKey)))
                        .MustVerifySignature()
                        .Decode(JwtToken);
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }

            }
        }
    }
}
