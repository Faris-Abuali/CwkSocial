using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity;

namespace CwkSocial.DataAccess.Configurations;

internal class UserProfileConfig : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // Make it BasicInfo an owned entity of UserProfile
        // This will make BasicInfo in the same table as UserProfile
        builder.OwnsOne(ui => ui.BasicInfo, opt =>
        {
            // Remove the prefix 'BasicInfo_' from the column names
            opt.Property(bi => bi.FirstName).HasColumnName("FirstName");
            opt.Property(bi => bi.LastName).HasColumnName("LastName");
            opt.Property(bi => bi.EmailAddress).HasColumnName("EmailAddress");
            opt.Property(bi => bi.Phone).HasColumnName("Phone");
            opt.Property(bi => bi.DateOfBirth).HasColumnName("DateOfBirth");
            opt.Property(bi => bi.CurrentCity).HasColumnName("CurrentCity");
        });

        //// Add foreign key to IdentityUser
        //builder.HasOne<IdentityUser>()
        //       .WithMany()
        //       .HasForeignKey(up => up.IdentityId)
        //       .OnDelete(DeleteBehavior.Cascade);
    }
}
