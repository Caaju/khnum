using Khnum.Domain.Authentication;

namespace Khnum.Application.Authentication.Login;

public sealed class LoginService
{
    private const string InvalidCredentialsMessage = "Credenciais inválidas ou usuário inativo.";

    private readonly ILoginRepository loginRepository;
    private readonly ILoginSessionRepository sessionRepository;
    private readonly IPasswordVerifier passwordVerifier;
    private readonly IClock clock;

    public LoginService(
        ILoginRepository loginRepository,
        ILoginSessionRepository sessionRepository,
        IPasswordVerifier passwordVerifier,
        IClock clock)
    {
        this.loginRepository = loginRepository;
        this.sessionRepository = sessionRepository;
        this.passwordVerifier = passwordVerifier;
        this.clock = clock;
    }

    public async Task<LoginResult> ExecuteAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        if (!Email.TryCreate(command.Email, out var email) || string.IsNullOrEmpty(command.Password) || command.Password.Length < 6)
        {
            return new LoginResult(LoginOutcome.ValidationError, "Os dados de login são inválidos.");
        }

        var login = await loginRepository.FindByEmailAsync(email!, cancellationToken);
        if (login is null || !login.IsActive || !passwordVerifier.Verify(command.Password, login.PasswordHash))
        {
            return new LoginResult(LoginOutcome.Failed, InvalidCredentialsMessage);
        }

        var session = LoginSession.Create(login.Id, clock.UtcNow);
        try
        {
            await sessionRepository.AddAsync(session, cancellationToken);
        }
        catch
        {
            return new LoginResult(LoginOutcome.PersistenceFailure, "Não foi possível concluir o login.");
        }

        return new LoginResult(LoginOutcome.Success, "Sessão criada com sucesso.", session.Token);
    }
}