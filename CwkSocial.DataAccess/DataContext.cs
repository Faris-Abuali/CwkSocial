using CwkSocial.DataAccess.Configurations;
using CwkSocial.DataAccess.Models;
using CwkSocial.Domain.Aggregates.PostAggregate;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CwkSocial.DataAccess;

public class DataContext : IdentityDbContext
{
    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(builder);
        modelBuilder.ApplyConfiguration(new PostCommentConfig());
        modelBuilder.ApplyConfiguration(new PostReactionConfig());
        modelBuilder.ApplyConfiguration(new UserProfileConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfig());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfig());
        modelBuilder.ApplyConfiguration(new PostConfig());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfig());
    }
}
