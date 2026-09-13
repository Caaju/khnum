using Khnum.Application.Authentication.Login;
using Khnum.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddKhnumInfrastructure(builder.Configuration);
builder.Services.AddScoped<LoginService>();

var app = builder.Build();

await app.Services.InitializeKhnumDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/v1/auth/login", async (LoginRequest? request, LoginService loginService, CancellationToken cancellationToken) =>
{
    var result = await loginService.ExecuteAsync(
        new LoginCommand(request?.Email, request?.Password),
        cancellationToken);

    var response = new LoginResponse(
        result.Outcome switch
        {
            LoginOutcome.Success => "SUCCESS",
            LoginOutcome.ValidationError => "VALIDATION_ERROR",
            _ => "FAILED"
        },
        result.Message,
        DateTimeOffset.UtcNow,
        result.Token is null ? null : new LoginResponseData(result.Token.Value));

    var statusCode = result.Outcome switch
    {
        LoginOutcome.Success => StatusCodes.Status200OK,
        LoginOutcome.ValidationError => StatusCodes.Status400BadRequest,
        LoginOutcome.Failed => StatusCodes.Status401Unauthorized,
        _ => StatusCodes.Status500InternalServerError
    };

    return Results.Json(response, statusCode: statusCode);
})
.WithName("Login");

app.Run();

public sealed record LoginRequest(string? Email, string? Password);

public sealed record LoginResponse(string Status, string Message, DateTimeOffset Dth, LoginResponseData? Data);

public sealed record LoginResponseData(Guid Token);

public partial class Program;
