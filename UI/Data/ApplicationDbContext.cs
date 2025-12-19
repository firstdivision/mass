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

    public DbSet<Story> Stories => Set<Story>();
    public DbSet<StoryInvite> StoryInvites => Set<StoryInvite>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Entry> Entries => Set<Entry>();
    public DbSet<WritingPrompt> WritingPrompts => Set<WritingPrompt>();

    public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();
    public DbSet<MassMassIdentityUserRole> MassMassIdentityUserRoles => Set<MassMassIdentityUserRole>();
    public DbSet<MassIdentityUser> MassIdentityUsers => Set<MassIdentityUser>();
    public DbSet<MassApplicationRole> MassApplicationRoles => Set<MassApplicationRole>();

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

        // Add the relationships for Story CreatedBy and Contributors
        builder.Entity<Story>()
            .HasOne(s => s.CreatedBy)
            .WithMany(u => u.CreatedStories)
            .HasForeignKey("CreatedById")
            .IsRequired();      

        builder.Entity<Story>()
            .HasMany(s => s.Contributors)
            .WithMany(u => u.ContributedStories)
            .UsingEntity<Dictionary<string, object>>(
                "StoryContributor",
                j => j
                    .HasOne<MassIdentityUser>()
                    .WithMany()
                    .HasForeignKey("ContributorId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Story>()
                    .WithMany()
                    .HasForeignKey("StoryId")
                    .OnDelete(DeleteBehavior.Cascade));

        // Add the relationship for Story LockedBy
        builder.Entity<Story>()
            .HasOne(s => s.LockedBy)
            .WithMany(u => u.LockedStories)
            .HasForeignKey("LockedById")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
