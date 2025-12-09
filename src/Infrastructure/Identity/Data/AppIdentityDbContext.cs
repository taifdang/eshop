using Infrastructure.Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Data;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, 
    ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {

    }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ForgetPassword> ForgetPassword { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {    
        base.OnModelCreating(builder);

        // ApplicationUser
        builder.Entity<ApplicationUser>()
            .Property(x => x.AvatarUrl)
            .HasMaxLength(500);

        builder.Entity<ApplicationUser>()
            .HasMany(x => x.RefreshTokens)
            .WithOne(y => y.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(x => x.UserRoles)
            .WithOne(y => y.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ApplicationRole
        builder.Entity<ApplicationRole>()
            .HasMany(r => r.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        // ApplicationUserRole
        builder.Entity<ApplicationUserRole>()
            .HasKey(x => new { x.UserId, x.RoleId });
        
        builder.Entity<ApplicationUserRole>()
            .HasOne(x => x.User)
            .WithMany(au => au.UserRoles)
            .HasForeignKey(aur => aur.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUserRole>()
            .HasOne(x => x.Role)
            .WithMany(ar => ar.UserRoles)
            .HasForeignKey(aur => aur.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ForgetPassword
        builder.Entity<ForgetPassword>()
            .ToTable(nameof(ForgetPassword));

        builder.Entity<ForgetPassword>()
            .HasKey(fp => fp.Id);
        builder.Entity<ForgetPassword>()
            .Property(fp => fp.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<ForgetPassword>()
            .Property(fp => fp.UserId)
            .IsRequired();

        builder.Entity<ForgetPassword>()
            .Property(fp => fp.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Entity<ForgetPassword>()
            .Property(fp => fp.OTP)
            .IsRequired()
            .HasMaxLength(10);

        builder.Entity<ForgetPassword>()
            .Property(fp => fp.DateTime)
            .IsRequired();

        builder.Entity<ForgetPassword>()
            .Property(fp => fp.Token)
            .IsRequired();

        // RefreshToken
        builder.Entity<RefreshToken>()
            .ToTable(nameof(RefreshToken));

        builder.Entity<RefreshToken>()
            .HasKey(rt => rt.Id);

        builder.Entity<RefreshToken>()
            .Property(rt => rt.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<RefreshToken>()
            .Property(rt => rt.UserId)
            .IsRequired();

        builder.Entity<RefreshToken>()
            .Property(rt => rt.Token)
            .IsRequired();

        builder.Entity<RefreshToken>()
            .HasOne(x => x.User)
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}