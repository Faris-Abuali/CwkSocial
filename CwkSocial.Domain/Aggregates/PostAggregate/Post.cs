using CwkSocial.Domain.Aggregates.UserProfileAggregate;

namespace CwkSocial.Domain.Aggregates.PostAggregate;

public class Post
{
    // private fields (backing fields)
    private readonly List<PostComment> _comments = new();

    private readonly List<PostReaction> _reactions = new();

    // properties
    public Guid PostId { get; private set; }

    public Guid UserProfileId { get; private set; } // Foreign key to UserProfile

    public UserProfile? UserProfile { get; private set; } // Navigation property for EF Core

    public string TextContent { get; private set; } = string.Empty;

    public DateTime CreatedDate { get; private set; }

    public DateTime LastModified { get; private set; }

    // Read-only properties: to prevent modification of the collection
    public IEnumerable<PostComment>? Comments => _comments;

    public IEnumerable<PostReaction>? Reactions => _reactions;

    private Post() { }

    // Factory methods
    public static Post Create(Guid userProfileId, string textContent)
    {
        // TODO: Add validation, error handling strategies, error notifications.

        return new Post
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            PostId = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    // public methods
    public void UpdatePostText(string textContent)
    {
        TextContent = textContent;
        LastModified = DateTime.UtcNow;
    }

    public void AddComment(PostComment comment)
    {
        _comments.Add(comment);
    }

    public void AddReaction(PostReaction reaction)
    {
        _reactions.Add(reaction);
    }

    public void RemoveComment(PostComment comment)
    {
        _comments.Remove(comment);
    }

    public void RemoveReaction(PostReaction reaction)
    {
        _reactions.Remove(reaction);
    }
}
