using Khnum.Application.Authentication.Login;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khnum.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddKhnumInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AuthDb") ?? "Data Source=khnum_auth.db;Cache=Shared";
        if (!connectionString.Contains("Foreign Keys", StringComparison.OrdinalIgnoreCase))
        {
            connectionString += ";Foreign Keys=True";
        }
        services.AddDbContext<Authentication.AuthDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddScoped<ILoginRepository, Authentication.LoginRepository>();
        services.AddScoped<ILoginSessionRepository, Authentication.LoginSessionRepository>();
        services.AddSingleton<IPasswordVerifier, Authentication.PasswordVerifier>();
        services.AddSingleton<IClock, Authentication.SystemClock>();
        return services;
    }

    public static async Task InitializeKhnumDatabaseAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Authentication.AuthDbContext>();
        await dbContext.Database.OpenConnectionAsync();
        await using var command = dbContext.Database.GetDbConnection().CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        await command.ExecuteNonQueryAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.CloseConnectionAsync();
    }
}