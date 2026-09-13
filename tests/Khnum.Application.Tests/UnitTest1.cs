using Khnum.Application.Authentication.Login;
using Khnum.Domain.Authentication;

namespace Khnum.Application.Tests;

public class LoginServiceTests
{
    private readonly DateTimeOffset now = new(2026, 9, 13, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task Valid_credentials_create_and_return_persisted_session_token()
    {
        var login = CreateLogin();
        var sessions = new FakeSessionRepository();
        var service = CreateService(login, sessions);

        var result = await service.ExecuteAsync(new LoginCommand("USER@KHNUM.IO", "SenhaForte@123"));

        Assert.Equal(LoginOutcome.Success, result.Outcome);
        Assert.Equal(sessions.Session!.Token, result.Token);
    }

    [Fact]
    public async Task Inactive_user_does_not_create_a_session()
    {
        var sessions = new FakeSessionRepository();
        var service = CreateService(CreateLogin(isActive: false), sessions);

        var result = await service.ExecuteAsync(new LoginCommand("user@khnum.io", "SenhaForte@123"));

        Assert.Equal(LoginOutcome.Failed, result.Outcome);
        Assert.Null(sessions.Session);
    }

    [Fact]
    public async Task Invalid_password_does_not_create_a_session()
    {
        var sessions = new FakeSessionRepository();
        var service = CreateService(CreateLogin(), sessions, passwordMatches: false);

        var result = await service.ExecuteAsync(new LoginCommand("user@khnum.io", "SenhaErrada!"));

        Assert.Equal(LoginOutcome.Failed, result.Outcome);
        Assert.Null(sessions.Session);
    }

    [Fact]
    public async Task Invalid_input_does_not_query_the_repository()
    {
        var repository = new FakeLoginRepository(CreateLogin());
        var service = CreateService(repository: repository);

        var result = await service.ExecuteAsync(new LoginCommand("invalid", ""));

        Assert.Equal(LoginOutcome.ValidationError, result.Outcome);
        Assert.Equal(0, repository.LookupCount);
    }

    [Fact]
    public async Task Persistence_failure_does_not_return_a_token()
    {
        var service = CreateService(CreateLogin(), new FakeSessionRepository(throws: true));

        var result = await service.ExecuteAsync(new LoginCommand("user@khnum.io", "SenhaForte@123"));

        Assert.Equal(LoginOutcome.PersistenceFailure, result.Outcome);
        Assert.Null(result.Token);
    }

    private LoginService CreateService(
        Khnum.Domain.Authentication.Login? login = null,
        FakeSessionRepository? sessions = null,
        bool passwordMatches = true,
        FakeLoginRepository? repository = null)
    {
        return new LoginService(
            repository ?? new FakeLoginRepository(login),
            sessions ?? new FakeSessionRepository(),
            new FakePasswordVerifier(passwordMatches),
            new FixedClock(now));
    }

    private static Khnum.Domain.Authentication.Login CreateLogin(bool isActive = true)
    {
        Email.TryCreate("user@khnum.io", out var email);
        return new Khnum.Domain.Authentication.Login(Guid.NewGuid(), email!, "hash", isActive);
    }

    private sealed class FakeLoginRepository(Khnum.Domain.Authentication.Login? login) : ILoginRepository
    {
        public int LookupCount { get; private set; }

        public Task<Khnum.Domain.Authentication.Login?> FindByEmailAsync(Email email, CancellationToken cancellationToken)
        {
            LookupCount++;
            return Task.FromResult(login);
        }
    }

    private sealed class FakeSessionRepository(bool throws = false) : ILoginSessionRepository
    {
        public LoginSession? Session { get; private set; }

        public Task AddAsync(LoginSession session, CancellationToken cancellationToken)
        {
            if (throws)
            {
                throw new InvalidOperationException();
            }

            Session = session;
            return Task.CompletedTask;
        }
    }

    private sealed class FakePasswordVerifier(bool matches) : IPasswordVerifier
    {
        public bool Verify(string password, string passwordHash) => matches;
    }

    private sealed class FixedClock(DateTimeOffset utcNow) : IClock
    {
        public DateTimeOffset UtcNow => utcNow;
    }
}
