using System.Security.Cryptography;
using System.Text;
using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class Password : ValueObject
{
    public string Hash { get; }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Password Create(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentException("Password cannot be empty", nameof(plainTextPassword));

        if (plainTextPassword.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long", nameof(plainTextPassword));

        var hash = HashPassword(plainTextPassword);
        return new Password(hash);
    }

    public static Password FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty", nameof(hash));

        return new Password(hash);
    }

    public bool Verify(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            return false;

        var hashToVerify = HashPassword(plainTextPassword);
        return Hash == hashToVerify;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "WalletoSalt2024"));
        return Convert.ToBase64String(hashedBytes);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }
}