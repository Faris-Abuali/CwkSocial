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
               .OnDelete(DeleteBehavior.SetNull);


        // Create unique key for PostId and UserProfileId
        // This will prevent a user from reacting to a post more than once
        builder.HasIndex(pr => new { pr.PostId, pr.UserProfileId })
               .IsUnique();
    }
}
