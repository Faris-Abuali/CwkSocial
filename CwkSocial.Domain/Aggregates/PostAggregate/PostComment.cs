using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using CwkSocial.Domain.Validators.PostsValidators;

namespace CwkSocial.Domain.Aggregates.PostAggregate;

public class PostComment
{
    public Guid CommentId { get; private set; }

    public Guid PostId { get; private set; } // Foreign key to Post

    public Guid? UserProfileId { get; private set; } // Foreign key to UserProfile

    public UserProfile? UserProfile { get; private set; } // Navigation property for EF Core

    public string Text { get; private set; } = string.Empty;

    public DateTime CreatedDate { get; private set; }

    public DateTime LastModified { get; private set; }

    private PostComment() { }

    // Factory methods

    /// <summary>
    /// Creates a new comment for a post
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="userProfileId"></param>
    /// <param name="text"></param>
    /// <returns>The newly created comment</returns>
    /// <exception cref="PostCommentNotValidException"></exception>
    public static PostComment Create(Guid postId, Guid userProfileId, string text)
    {
        var validator = new PostCommentValidator();

        var postComment = new PostComment
        {
            PostId = postId,
            UserProfileId = userProfileId,
            Text = text,
            CommentId = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };

        var validationResult = validator.Validate(postComment);

        if (validationResult.IsValid) return postComment;

        var exception = new PostCommentNotValidException("Post comment is invalid")
        {
            ValidationErrors = validationResult.Errors.ConvertAll(e => e.ErrorMessage)
        };

        throw exception;
    }

    // public methods
    public void UpdateCommentText(string text)
    {
        Text = text;
        LastModified = DateTime.UtcNow;
    }

    //public void Delete()
    //{
    //    CommentId = Guid.Empty;
    //    PostId = Guid.Empty;
    //    UserProfileId = Guid.Empty;
    //}
}
