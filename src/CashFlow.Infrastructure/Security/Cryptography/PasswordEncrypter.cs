using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security.Cryptography;

public class PasswordEncrypter : IPasswordEncrypter
{
    public string Encrypt(string password)
    {
        var passwordHash = BC.HashPassword(password);

        return passwordHash;
    }
    
    public bool Verify(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}