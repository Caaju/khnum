namespace Khnum.Domain.Authentication;

public sealed class LoginSession
{
    public const int DurationMinutes = 30;

    private LoginSession()
    {
    }

    private LoginSession(Guid token, Guid loginId, DateTimeOffset createdAtUtc, DateTimeOffset expiresAtUtc)
    {
        if (expiresAtUtc <= createdAtUtc)
        {
            throw new ArgumentException("Session expiration must be after creation.", nameof(expiresAtUtc));
        }

        Token = token;
        LoginId = loginId;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        IsRevoked = false;
    }

    public Guid Token { get; private set; }
    public Guid LoginId { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public bool IsRevoked { get; private set; }

    public static LoginSession Create(Guid loginId, DateTimeOffset createdAtUtc)
    {
        return Create(loginId, createdAtUtc, createdAtUtc.AddMinutes(DurationMinutes));
    }

    public static LoginSession Create(Guid loginId, DateTimeOffset createdAtUtc, DateTimeOffset expiresAtUtc)
    {
        return new LoginSession(Guid.NewGuid(), loginId, createdAtUtc, expiresAtUtc);
    }
}