[TestFixture]
public class PasswordHasherTests
{
    private const string ValidPassword = "SuperSecretPassword123!";

    [Test]
    public void HashPassword_ValidPassword_ReturnsFormattedString()
    {
        string result = PasswordHasher.HashPassword(ValidPassword);

        Assert.That(result, Is.Not.Null.And.Not.Empty);

        string[] parts = result.Split(':');
        Assert.That(parts.Length, Is.EqualTo(2), "Output should contain salt and hash separated by a colon.");
        Assert.DoesNotThrow(() => Convert.FromBase64String(parts[0]), "Salt should be valid Base64.");
        Assert.DoesNotThrow(() => Convert.FromBase64String(parts[1]), "Hash should be valid Base64.");
    }

    [Test]
    public void HashPassword_SamePasswordTwice_GeneratesDifferentHashesDueToSalting()
    {
        string hash1 = PasswordHasher.HashPassword(ValidPassword);
        string hash2 = PasswordHasher.HashPassword(ValidPassword);

        Assert.That(hash1, Is.Not.EqualTo(hash2), "Hashes for the same password must differ because salts are unique.");
    }

    [TestCase(null)]
    [TestCase("")]
    public void HashPassword_NullOrEmptyPassword_ThrowsArgumentNullException(string? invalidPassword)
    {
        Assert.Throws<ArgumentNullException>(() => PasswordHasher.HashPassword(invalidPassword));
    }


    [Test]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        string storedHash = PasswordHasher.HashPassword(ValidPassword);

        bool isValid = PasswordHasher.VerifyPassword(ValidPassword, storedHash);

        Assert.That(isValid, Is.True);
    }


    [Test]
    public void VerifyPassword_WrongPassword_ReturnsFalse()
    {
        string storedHash = PasswordHasher.HashPassword(ValidPassword);

        bool isValid = PasswordHasher.VerifyPassword("WrongPassword123!", storedHash);

        Assert.That(isValid, Is.False);
    }

    [Test]
    public void VerifyPassword_CaseSensitivePassword_ReturnsFalse()
    {
        string storedHash = PasswordHasher.HashPassword("Password");

        bool isValid = PasswordHasher.VerifyPassword("password", storedHash);

        Assert.That(isValid, Is.False, "Verification must be case-sensitive.");
    }


    [TestCase(null, "validSalt:validHash")]
    [TestCase("", "validSalt:validHash")]
    [TestCase("SuperSecretPassword123!", null)]
    [TestCase("SuperSecretPassword123!", "")]
    public void VerifyPassword_NullOrEmptyInputs_ReturnsFalse(string? password, string? storedHash)
    {
        bool result = PasswordHasher.VerifyPassword(password, storedHash);
        Assert.That(result, Is.False);
    }


    [TestCase("InvalidFormatNoColon")]
    [TestCase("Too:Many:Colons:In:String")]
    public void VerifyPassword_MalformedStoredHash_ReturnsFalse(string malformedHash)
    {
        bool result = PasswordHasher.VerifyPassword(ValidPassword, malformedHash);

        Assert.That(result, Is.False);
    }

    [Test]
    public void VerifyPassword_CorruptedBase64Hash_ThrowsFormatException()
    {
        string invalidBase64 = "NotBase64!!!:NotBase64!!!";

        Assert.Throws<FormatException>(() => PasswordHasher.VerifyPassword(ValidPassword, invalidBase64));
    }
}