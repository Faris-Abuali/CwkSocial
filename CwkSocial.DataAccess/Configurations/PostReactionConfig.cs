using Microsoft.EntityFrameworkCore;
using CwkSocial.Domain.Aggregates.PostAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CwkSocial.DataAccess.Configurations;

internal class PostReactionConfig : IEntityTypeConfiguration<PostReaction>
{
    public void Configure(EntityTypeBuilder<PostReaction> builder)
    {
        builder.HasKey(pr => pr.ReactionId);

        // Define foreign key to UserProfile with OnDelete behavior
        builder.HasOne(pc => pc.UserProfile)
               .WithMany()
               .HasForeignKey(pc => pc.UserProfileId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
