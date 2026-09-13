using Khnum.Application.Authentication.Login;
using Khnum.Domain.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Khnum.Infrastructure.Authentication;

public sealed class LoginRepository(AuthDbContext dbContext) : ILoginRepository
{
    public Task<Login?> FindByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return dbContext.Logins.SingleOrDefaultAsync(login => login.Email == email, cancellationToken);
    }
}

public sealed class LoginSessionRepository(AuthDbContext dbContext) : ILoginSessionRepository
{
    public async Task AddAsync(LoginSession session, CancellationToken cancellationToken)
    {
        dbContext.LoginSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}