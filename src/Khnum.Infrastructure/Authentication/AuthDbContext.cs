using Khnum.Domain.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Khnum.Infrastructure.Authentication;

public sealed class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<Login> Logins => Set<Login>();
    public DbSet<LoginSession> LoginSessions => Set<LoginSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Login>(entity =>
        {
            entity.ToTable("logins");
            entity.HasKey(login => login.Id);
            entity.Property(login => login.Id).HasConversion<string>();
            entity.Property(login => login.Email)
                .HasConversion(email => email.Value, value => ConvertEmail(value))
                .HasColumnName("email")
                .IsRequired();
            entity.HasIndex(login => login.Email).IsUnique();
            entity.Property(login => login.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(login => login.IsActive).HasColumnName("is_active").IsRequired();
            entity.Property(login => login.CreatedAtUtc).HasColumnName("created_at_utc").IsRequired();
            entity.Property(login => login.UpdatedAtUtc).HasColumnName("updated_at_utc").IsRequired();
            entity.Property(login => login.Email).UseCollation("NOCASE");
        });

        modelBuilder.Entity<LoginSession>(entity =>
        {
            entity.ToTable("login_sessions");
            entity.HasKey(session => session.Token);
            entity.Property(session => session.Token).HasConversion<string>().HasColumnName("token");
            entity.Property(session => session.LoginId).HasConversion<string>().HasColumnName("login_id");
            entity.Property(session => session.CreatedAtUtc).HasColumnName("created_at_utc");
            entity.Property(session => session.ExpiresAtUtc).HasColumnName("expires_at_utc");
            entity.Property(session => session.IsRevoked).HasColumnName("is_revoked").IsRequired();
            entity.HasIndex(session => session.LoginId);
            entity.HasOne<Login>()
                .WithMany()
                .HasForeignKey(session => session.LoginId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static Email ConvertEmail(string value)
    {
        if (!Email.TryCreate(value, out var email))
        {
            throw new InvalidOperationException("Stored email is invalid.");
        }

        return email!;
    }
}