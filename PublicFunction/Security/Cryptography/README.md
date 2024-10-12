# PublicFunction.Security.Cryptography.ISecurityEncryption

### Explanation of Encryption Types

1.  **Symmetric Encryption (AES)**:
    -   AES (Advanced Encryption Standard) is a symmetric encryption algorithm where the same key is used for both encryption and decryption.
    -   The `EncryptAES` and `DecryptAES` methods demonstrate how to use AES encryption with a given key and initialization vector (IV).
2.  **Asymmetric Encryption (RSA)**:
    -   RSA (Rivest-Shamir-Adleman) is an asymmetric encryption algorithm that uses a pair of keys (public and private) to encrypt and decrypt data. The public key is used for encryption, while the private key is used for decryption.
    -   The `EncryptRSA` and `DecryptRSA` methods demonstrate RSA encryption and decryption with a public/private key pair.
3.  **Hashing (SHA-256)**:
    -   Hashing is a one-way process where data is converted into a fixed-size string or number, typically used for verifying data integrity.
    -   The `Hash` method can be used with various hashing algorithms like SHA-256, SHA-512, etc., to create a hash of the input data.
4.  **Digital Signatures**:
    -   Digital signatures ensure the authenticity and integrity of data. The `SignData` and `VerifySignature` methods demonstrate how to sign data using a private key and verify it using the corresponding public key (RSA-based)

    Here’s how you can use the `SecurityEncryption` class for various cryptographic operations, including AES encryption, RSA encryption, hashing, and digital signatures. I'll explain step by step and provide an example of how to use it.

### 1. **Key Generation**:

For **AES encryption** (symmetric encryption), you need to generate an encryption key and an initialization vector (IV). For **RSA encryption** (asymmetric encryption), you need a public key and a private key pair.

### 2. **AES Encryption and Decryption (Symmetric Encryption)**:

For symmetric encryption, AES is used. You encrypt and decrypt data using the same key and an initialization vector (IV).

### 3. **RSA Encryption and Decryption (Asymmetric Encryption)**:

For RSA encryption, you use a public key to encrypt and a private key to decrypt the data.

### 4. **Hashing**:

Hashing generates a fixed-size digest (hash) of the data. It’s one-way, meaning the original data cannot be retrieved from the hash.

### 5. **Digital Signature and Signature Verification**:

To sign data, use a private key. To verify the signature, use the corresponding public key.

### Example Usage of `SecurityEncryption`:

```csharp
using System;
using System.Security.Cryptography;

class Program
{
    static void Main()
    {
        // 1. Create an instance of SecurityEncryption
        SecurityEncryption security = new SecurityEncryption();

        // --- AES (Symmetric Encryption) ---
        // Generate a random AES key (256-bit key) and IV
        byte[] aesKey = security.GenerateSymmetricKey(32);  // 32 bytes = 256 bits
        byte[] aesIV = security.GenerateSymmetricKey(16);   // 16 bytes = 128 bits (IV size for AES)

        string plainText = "This is a secret message.";

        // Encrypt the plaintext using AES
        byte[] encryptedTextAES = security.EncryptAES(plainText, aesKey, aesIV);
        Console.WriteLine("Encrypted AES: " + Convert.ToBase64String(encryptedTextAES));

        // Decrypt the encrypted AES data
        string decryptedTextAES = security.DecryptAES(encryptedTextAES, aesKey, aesIV);
        Console.WriteLine("Decrypted AES: " + decryptedTextAES);

        // --- RSA (Asymmetric Encryption) ---
        // Generate RSA keys (Public and Private keys)
        using (RSA rsa = RSA.Create())
        {
            string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            // Encrypt data with RSA public key
            byte[] encryptedTextRSA = security.EncryptRSA(plainText, publicKey);
            Console.WriteLine("Encrypted RSA: " + Convert.ToBase64String(encryptedTextRSA));

            // Decrypt the RSA encrypted data with the private key
            string decryptedTextRSA = security.DecryptRSA(encryptedTextRSA, privateKey);
            Console.WriteLine("Decrypted RSA: " + decryptedTextRSA);
        }

        // --- Hashing (SHA-256) ---
        string hashText = security.Hash(plainText, SHA256.Create());
        Console.WriteLine("SHA-256 Hash: " + hashText);

        // --- Digital Signature ---
        // Generate RSA keys for signing (Public and Private)
        using (RSA rsa = RSA.Create())
        {
            string privateKeyForSigning = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            string publicKeyForSigning = Convert.ToBase64String(rsa.ExportRSAPublicKey());

            // Sign data using the private key
            byte[] signature = security.SignData(Encoding.UTF8.GetBytes(plainText), privateKeyForSigning);
            Console.WriteLine("Digital Signature: " + Convert.ToBase64String(signature));

            // Verify the signature using the public key
            bool isVerified = security.VerifySignature(Encoding.UTF8.GetBytes(plainText), signature, publicKeyForSigning);
            Console.WriteLine("Signature Verified: " + isVerified);
        }
    }
}

```

### Explanation:

1.  **AES (Symmetric Encryption)**:
    
    -   **GenerateSymmetricKey**: This generates a 256-bit AES key and a 128-bit initialization vector (IV).
    -   **EncryptAES**: Encrypts the plain text using the AES algorithm, key, and IV.
    -   **DecryptAES**: Decrypts the cipher text back to the original plain text using the same AES key and IV.
2.  **RSA (Asymmetric Encryption)**:
    
    -   **EncryptRSA**: Encrypts the plain text using the public key.
    -   **DecryptRSA**: Decrypts the cipher text using the private key.
3.  **Hashing (SHA-256)**:
    
    -   **Hash**: Generates a SHA-256 hash of the input string, which is a fixed-length string (digest) of the input data.
4.  **Digital Signature**:
    
    -   **SignData**: Signs the data using the private key and returns the signature.
    -   **VerifySignature**: Verifies the digital signature using the public key.

### Output Example:
```yaml
Encrypted AES: eU5d6tD+G4tweI6vG0Ud7Q==
Decrypted AES: This is a secret message.
Encrypted RSA: WcDd75x2/tw4wF27uwm4iPAkF+I1mtGl6jOfqLk5l4ke+ETt+Ev9Zm0zE7QFfH6EK9UbWp4M32XU...
Decrypted RSA: This is a secret message.
SHA-256 Hash: 2c6ee24b09816a6f14f95d1698b24ead1eaa6a96c6aab19f99d3f997bd9b3f34
Digital Signature: M5aU0ZYNq1m6...
Signature Verified: True

```
This example demonstrates how to use the class for symmetric and asymmetric encryption, hashing, and digital signatures. You can modify the key sizes, encryption algorithms, or hashing algorithms based on your security requirements.
