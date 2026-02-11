using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AfigoBackend.Infraestructure.Util;

public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string hashed, string plain);
}

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    // Usa el hasher de Identity internamente
    private readonly PasswordHasher<object> _inner = new();

    public string Hash(string password)
        => _inner.HashPassword(new object(), password);

    public bool Verify(string hashed, string plain)
    {
        var result = _inner.VerifyHashedPassword(new object(), hashed, plain);
        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}
