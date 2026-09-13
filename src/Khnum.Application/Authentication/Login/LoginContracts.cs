using Khnum.Domain.Authentication;

namespace Khnum.Application.Authentication.Login;

public sealed record LoginCommand(string? Email, string? Password);

public enum LoginOutcome
{
    Success,
    ValidationError,
    Failed,
    PersistenceFailure
}

public sealed record LoginResult(LoginOutcome Outcome, string Message, Guid? Token = null);

public interface ILoginRepository
{
    Task<Khnum.Domain.Authentication.Login?> FindByEmailAsync(Email email, CancellationToken cancellationToken);
}

public interface ILoginSessionRepository
{
    Task AddAsync(LoginSession session, CancellationToken cancellationToken);
}

public interface IPasswordVerifier
{
    bool Verify(string password, string passwordHash);
}

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}