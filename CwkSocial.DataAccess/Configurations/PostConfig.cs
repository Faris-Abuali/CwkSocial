using CwkSocial.Domain.Aggregates.PostAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CwkSocial.DataAccess.Configurations;

internal class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(p => p.PostId);

        // Define foreign key to UserProfile with OnDelete behavior
        builder.HasOne(p => p.UserProfile)
               .WithMany()
               .HasForeignKey(p => p.UserProfileId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
