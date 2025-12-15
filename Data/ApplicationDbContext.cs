using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace mass.Data;

public class MassDbContext : IdentityDbContext<MassIdentityUser, MassApplicationRole, string, 
    IdentityUserClaim<string>, MassMassIdentityUserRole, IdentityUserLogin<string>, 
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
{
    public MassDbContext(DbContextOptions<MassDbContext> options)
        : base(options)
    {
    }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    public DbSet<MassMassIdentityUserRole> MassMassIdentityUserRoles { get; set; } = null!;
    public DbSet<MassIdentityUser> MassIdentityUsers { get; set; } = null!;
    public DbSet<MassApplicationRole> MassApplicationRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MassMassIdentityUserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.Entity<MassMassIdentityUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.Entity<MassMassIdentityUserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}
