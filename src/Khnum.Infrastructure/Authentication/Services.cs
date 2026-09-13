using Khnum.Application.Authentication.Login;
using Khnum.Domain.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Khnum.Infrastructure.Authentication;

public sealed class PasswordVerifier : IPasswordVerifier
{
    private readonly PasswordHasher<object> hasher = new();

    public bool Verify(string password, string passwordHash)
    {
        return hasher.VerifyHashedPassword(new object(), passwordHash, password) == PasswordVerificationResult.Success;
    }
}

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}