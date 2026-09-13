using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Khnum.Domain.Authentication;
using Khnum.Infrastructure.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace Khnum.IntegrationTests;

public sealed class LoginEndpointTests : IAsyncLifetime
{
    private readonly string databasePath = Path.Combine(Path.GetTempPath(), $"khnum-{Guid.NewGuid():N}.db");
    private WebApplicationFactory<Program> factory = null!;
    private HttpClient client = null!;
    private Guid loginId;

    public async Task InitializeAsync()
    {
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }

        factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.UseSetting("ConnectionStrings:AuthDb", $"Data Source={databasePath};Cache=Shared");
                builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:AuthDb"] = $"Data Source={databasePath};Cache=Shared"
                    }));
            });
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost")
        });

        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        Email.TryCreate("usuario@khnum.io", out var email);
        loginId = Guid.NewGuid();
        var hash = new PasswordHasher<object>().HashPassword(new object(), "SenhaForte@123");
        db.Logins.Add(new Login(loginId, email!, hash, true));
        await db.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        client.Dispose();
        await factory.DisposeAsync();
        SqliteConnection.ClearAllPools();
        File.Delete(databasePath);
    }

    [Fact]
    public async Task Login_persists_and_returns_the_same_uuid_session_token()
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new
        {
            email = "USUARIO@KHNUM.IO",
            password = "SenhaForte@123"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("SUCCESS", body.RootElement.GetProperty("status").GetString());
        Assert.Equal("Sessão criada com sucesso.", body.RootElement.GetProperty("message").GetString());
        var token = body.RootElement.GetProperty("data").GetProperty("token").GetGuid();
        Assert.Equal(4, token.ToString().Split('-')[2][0] - '0');

        await using var scope = factory.Services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
        var session = await db.LoginSessions.SingleAsync(item => item.Token == token);
        Assert.Equal(loginId, session.LoginId);
        Assert.True(session.ExpiresAtUtc > session.CreatedAtUtc);
    }
}
