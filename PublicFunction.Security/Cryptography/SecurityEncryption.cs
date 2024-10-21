using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Security.Cryptography
{
    public interface ISecurityEncryption
    {
        // Method to generate a random symmetric encryption key (AES)
        byte[] GenerateSymmetricKey(int size = 32);

        // Method to encrypt data using AES (symmetric encryption)
        byte[] EncryptAES(string plainText, byte[] key, byte[] iv);

        // Method to decrypt data using AES (symmetric encryption)
        string DecryptAES(byte[] cipherText, byte[] key, byte[] iv);

        // Method to encrypt data using RSA (asymmetric encryption)
        byte[] EncryptRSA(string plainText, string publicKey);

        // Method to decrypt data using RSA (asymmetric encryption)
        string DecryptRSA(byte[] cipherText, string privateKey);

        // Method to hash data (e.g., SHA-256)
        string Hash(string input, HashAlgorithm algorithm);

        // Method to sign data using a private key (RSA)
        byte[] SignData(byte[] data, string privateKey);

        // Method to verify the signature of data using the public key (RSA)
        bool VerifySignature(byte[] data, byte[] signature, string publicKey);
    }
    public class SecurityEncryption : ISecurityEncryption
    {
        // Generates a symmetric encryption key for AES (Advanced Encryption Standard)
        public byte[] GenerateSymmetricKey(int size = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[size];  // Default size is 32 bytes (256-bit AES key)
                randomNumberGenerator.GetBytes(key);  // Fill the key with random bytes
                return key;
            }
        }

        // Encrypts a plaintext using AES encryption with a provided key and IV
        public byte[] EncryptAES(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;  // Set the encryption key
                aes.IV = iv;    // Set the initialization vector (IV)

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(cs))
                        {
                            writer.Write(plainText);
                        }
                    }
                    return ms.ToArray();  // Return the encrypted data as a byte array
                }
            }
        }

        // Decrypts a cipher text using AES with the provided key and IV
        public string DecryptAES(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;  // Set the decryption key
                aes.IV = iv;    // Set the initialization vector (IV)

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(cipherText))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cs))
                        {
                            return reader.ReadToEnd();  // Return the decrypted string
                        }
                    }
                }
            }
        }

        // Encrypts data using RSA (asymmetric encryption) with the given public key
        public byte[] EncryptRSA(string plainText, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

                byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
                return rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);  // Use RSA-OAEP for encryption
            }
        }

        // Decrypts data using RSA with the provided private key
        public string DecryptRSA(byte[] cipherText, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                byte[] decryptedData = rsa.Decrypt(cipherText, RSAEncryptionPadding.OaepSHA256);  // Use RSA-OAEP for decryption
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        // Hashes input data using a specified hash algorithm (e.g., SHA-256, SHA-512)
        public string Hash(string input, HashAlgorithm algorithm)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);  // Convert input to byte array
            byte[] hashBytes = algorithm.ComputeHash(data);  // Compute hash
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);  // Return hex string
        }

        // Signs data using RSA and the given private key
        public byte[] SignData(byte[] data, string privateKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

                // Sign the data with the private key using SHA-256 and PKCS1 padding
                return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }

        // Verifies the digital signature of data using the given public key
        public bool VerifySignature(byte[] data, byte[] signature, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

                // Verify the signature with the public key
                return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}
