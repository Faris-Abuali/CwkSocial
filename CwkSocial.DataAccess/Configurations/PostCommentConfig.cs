using Microsoft.EntityFrameworkCore;
using CwkSocial.Domain.Aggregates.PostAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CwkSocial.DataAccess.Configurations;

internal class PostCommentConfig : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder.HasKey(pc => pc.CommentId);

        // Define foreign key to UserProfile with OnDelete behavior
        builder.HasOne(pc => pc.UserProfile)
               .WithMany()
               .HasForeignKey(pc => pc.UserProfileId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
