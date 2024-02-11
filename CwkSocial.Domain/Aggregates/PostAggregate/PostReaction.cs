namespace CwkSocial.Domain.Aggregates.PostAggregate;

public class PostReaction
{
    public Guid ReactionId { get; private set; }

    public Guid PostId { get; private set; } // Foreign key to Post

    public Guid UserProfileId { get; private set; } // Foreign key to UserProfile

    public ReactionType ReactionType { get; private set; }

    public DateTime CreatedDate { get; private set; }

    private PostReaction() { }

    // Factory methods
    public static PostReaction Create(Guid postId, Guid userProfileId, ReactionType reactionType)
    {
        // TODO: Add validation, error handling strategies, error notifications.

        return new PostReaction
        {
            PostId = postId,
            UserProfileId = userProfileId,
            ReactionType = reactionType,
            ReactionId = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow
        };
    }
}
