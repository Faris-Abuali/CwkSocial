using CwkSocial.DataAccess.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CwkSocial.DataAccess.Configurations;

internal class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //// Let EFCore know that ApplicationUser extends IdentityUser
        //builder.HasBaseType<IdentityUser>();

        // Specify the key property explicitly
        //builder.HasKey(u => u.Id);

        // Add foreign key to UserProfile
        builder.HasMany<UserProfile>()
               .WithOne()
               .HasForeignKey(up => up.IdentityId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
