using IdentityService.Infrastructure.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

public class IdentityContext(DbContextOptions<IdentityContext> options) 
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    public DbSet<PasswordHistory> PasswordHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            entity.HasMany(u => u.PasswordHistories)
                .WithOne(ph => ph.User)
                .HasForeignKey(ph => ph.UserId)
                .IsRequired();
        });

        builder.Entity<UserRole>(entity =>
        {
            entity.HasOne(ur => ur.Role)
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        builder.Entity<PasswordHistory>(entity =>
        {
            entity.HasKey(ph => ph.Id);
            entity.Property(ph => ph.PasswordHash).IsRequired();
        });
    }
}
