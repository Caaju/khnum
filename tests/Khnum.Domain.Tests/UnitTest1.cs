using Khnum.Domain.Authentication;

namespace Khnum.Domain.Tests;

public class AuthenticationTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Email_rejects_invalid_values(string? value)
    {
        Assert.False(Email.TryCreate(value, out _));
    }

    [Fact]
    public void Email_normalizes_for_comparison()
    {
        Assert.True(Email.TryCreate("  USER@KHNUM.IO ", out var email));

        Assert.Equal("user@khnum.io", email!.Value);
    }

    [Fact]
    public void Login_session_expires_after_creation_for_thirty_minutes()
    {
        var createdAt = new DateTimeOffset(2026, 9, 13, 12, 0, 0, TimeSpan.Zero);

        var session = LoginSession.Create(Guid.NewGuid(), createdAt);

        Assert.True(session.ExpiresAtUtc > session.CreatedAtUtc);
        Assert.Equal(createdAt.AddMinutes(30), session.ExpiresAtUtc);
        Assert.NotEqual(Guid.Empty, session.Token);
        Assert.Equal(4, session.Token.ToString().Split('-')[2][0] - '0');
    }

    [Fact]
    public void Login_session_rejects_expiration_before_creation()
    {
        var createdAt = DateTimeOffset.UtcNow;

        Assert.Throws<ArgumentException>(() => LoginSession.Create(
            Guid.NewGuid(), createdAt, createdAt.AddMinutes(-1)));
    }
}
