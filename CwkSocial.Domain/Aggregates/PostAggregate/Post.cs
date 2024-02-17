using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using CwkSocial.Domain.Validators.PostsValidators;

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
        var validator = new PostValidator();

        var post = new Post
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            PostId = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var validationResult = validator.Validate(post);

        if (validationResult.IsValid) return post;

        var exception = new UserProfileNotValidException("Post is not valid")
        {
            ValidationErrors = validationResult.Errors.ConvertAll(e => e.ErrorMessage)
        };

        throw exception;
    }

    // public methods
    public void UpdatePostText(string textContent)
    {
        if (string.IsNullOrWhiteSpace(textContent))
        {
            var exception = new PostNotValidException("Cannot update post. Post text cannot be empty")
            {
                ValidationErrors = ["Post text cannot be empty"]
            };

            throw exception;
        }

        TextContent = textContent;
        LastModified = DateTime.UtcNow;
    }

    // Note: Since we already validated the PostComment.Create method,
    // and since it is the only method that can create a PostComment,
    // we don't need to validate the PostComment object here.
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
