# JWTToken Class Documentation

## Overview
The `JWTToken` class provides a structure for creating and decrypting JSON Web Tokens (JWT) using various algorithms. It includes nested classes and interfaces to handle different JWT algorithms such as HS256, HS384, HS512, RS256, RS384, and RS512.

## Classes and Interfaces

### TokenBaseStructure
This class defines the basic structure of a JWT token.

# JWTToken Class Documentation

## Overview
The `JWTToken` class provides a structure for creating and decrypting JSON Web Tokens (JWT) using various algorithms. It includes nested classes and interfaces to handle different JWT algorithms.

## Classes and Interfaces

### TokenBaseStructure
This class defines the basic structure of a JWT token.
- **Properties:**
  - `string uid`: User ID
  - `string iss`: Issuer
  - `string aud`: Audience
  - `double iat`: Issued At
  - `double nbf`: Not Before
  - `double exp`: Expiration Time

### IJWTAlgorithm
An interface for JWT algorithms.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### HS256
Implements the `IJWTAlgorithm` interface using the HMAC SHA-256 algorithm.
- **Constructor:**
  - `HS256(string secret)`: Initializes with a secret key.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### HS384
Implements the `IJWTAlgorithm` interface using the HMAC SHA-384 algorithm.
- **Constructor:**
  - `HS384(string secret)`: Initializes with a secret key.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### HS512
Implements the `IJWTAlgorithm` interface using the HMAC SHA-512 algorithm.
- **Constructor:**
  - `HS512(string secret)`: Initializes with a secret key.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### RS256
Implements the `IJWTAlgorithm` interface using the RSA SHA-256 algorithm.
- **Constructor:**
  - `RS256()`: Initializes with RSA keys.
- **Properties:**
  - `RSAParameters PrivateKey`: Gets or sets the private key.
  - `RSAParameters PublicKey`: Gets or sets the public key.
  - `string PrivateKey_XML`: Gets or sets the private key in XML format.
  - `string PublicKey_XML`: Gets or sets the public key in XML format.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### RS384
Implements the `IJWTAlgorithm` interface using the RSA SHA-384 algorithm.
- **Constructor:**
  - `RS384()`: Initializes with RSA keys.
- **Properties:**
  - `RSAParameters PrivateKey`: Gets or sets the private key.
  - `RSAParameters PublicKey`: Gets or sets the public key.
  - `string PrivateKey_XML`: Gets or sets the private key in XML format.
  - `string PublicKey_XML`: Gets or sets the public key in XML format.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.
  - `T Decrypt<T>(string JwtToken, string Issuer, string Audience, bool ValidateLifetime, bool ValidateIssuerSigningKey) where T : TokenBaseStructure`: Decrypts a JWT token.

### RS512
Implements the `IJWTAlgorithm` interface using the RSA SHA-512 algorithm.
- **Constructor:**
  - `RS512()`: Initializes with RSA keys.
- **Properties:**
  - `RSAParameters PrivateKey`: Gets or sets the private key.
  - `RSAParameters PublicKey`: Gets or sets the public key.
  - `string PrivateKey_XML`: Gets or sets the private key in XML format.
  - `string PublicKey_XML`: Gets or sets the public key in XML format.
- **Methods:**
  - `JwtBuilder Create(string Issuer, string Audience, DateTime NotBefore, DateTime ExpirationTime)`: Creates a JWT token.

## Usage Examples

### Creating a JWT Token with HS256
```csharp
var hs256 = new JWTToken.HS256("your-secret-key");
var token = hs256.Create("issuer", "audience", DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
                 .Encode();


# Decrypting a JWT Token with HS256

```csharp
var hs256 = new JWTToken.HS256("your-secret-key");
var tokenData = hs256.Decrypt<JWTToken.TokenBaseStructure>("your-jwt-token", "issuer", "audience", true, true);

# Creating a JWT Token with RS256

```csharp
var rs256 = new JWTToken.RS256();
rs256.PrivateKey_XML = "your-private-key-xml";
rs256.PublicKey_XML = "your-public-key-xml";
var token = rs256.Create("issuer", "audience", DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
                 .Encode();

# Decrypting a JWT Token with RS256

```csharp
var rs256 = new JWTToken.RS256();
rs256.PrivateKey_XML = "your-private-key-xml";
rs256.PublicKey_XML = "your-public-key-xml";
var tokenData = rs256.Decrypt<JWTToken.TokenBaseStructure>("your-jwt-token", "issuer", "audience", true, true);


