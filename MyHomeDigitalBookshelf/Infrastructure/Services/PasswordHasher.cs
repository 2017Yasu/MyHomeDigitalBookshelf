using System.Security.Cryptography;
using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Services;

[SingletonService]
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        using (var salt = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
        {
            var hash = salt.GetBytes(HashSize);
            var saltBytes = salt.Salt;

            var hashWithSalt = new byte[SaltSize + HashSize];
            Array.Copy(saltBytes, 0, hashWithSalt, 0, SaltSize);
            Array.Copy(hash, 0, hashWithSalt, SaltSize, HashSize);

            return Convert.ToBase64String(hashWithSalt);
        }
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

        try
        {
            var hashWithSalt = Convert.FromBase64String(passwordHash);

            if (hashWithSalt.Length != SaltSize + HashSize)
                return false;

            var salt = new byte[SaltSize];
            Array.Copy(hashWithSalt, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashWithSalt[i + SaltSize] != hash[i])
                        return false;
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
