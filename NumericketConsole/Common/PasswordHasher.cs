using System.Security.Cryptography;

/// <summary>
/// Provides secure, thread-safe methods for hashing and verifying passwords using PBKDF2.
/// </summary>
public static class PasswordHasher
{
    // 128-bit salt provides strong uniqueness per user
    private const int SaltSize = 16;

    // 256-bit output hash size
    private const int HashSize = 32;

    // High iteration count to make brute-force and hardware-accelerated attacks computationally expensive
    private const int Iterations = 350000;

    // Standard cryptographic hash algorithm
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    /**
     * PUBLIC METHODS
     */

    /// <summary>
    /// Hashes a plain-text password with a unique, cryptographically secure random salt.
    /// </summary>
    /// <param name="password">The plain-text password provided by the user.</param>
    /// <returns>A formatted string containing the Base64-encoded salt and hash separated by a colon.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the password input is null or empty.</exception>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

        // Generate a cryptographically secure random salt (unique for every call)
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Derive the hash key using PBKDF2 (HMACSHA256)
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            HashSize
        );

        // Store salt and hash together in a single payload string: "SaltInBase64:HashInBase64"
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies an entered plain-text password against a previously generated salt+hash string.
    /// </summary>
    /// <param name="password">The plain-text password supplied during login attempt.</param>
    /// <param name="storedHash">The combined salt and hash string retrieved from the database.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            return false;

        // Parse the stored combined string into salt and expected hash components
        string[] parts = storedHash.Split(':');
        if (parts.Length != 2)
            return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedHash = Convert.FromBase64String(parts[1]);

        // Recompute the hash for the incoming password using the retrieved salt
        byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            expectedHash.Length
        );

        // Compare using fixed-time comparison to prevent timing side-channel attacks
        return CryptographicOperations.FixedTimeEquals(expectedHash, computedHash);
    }
}