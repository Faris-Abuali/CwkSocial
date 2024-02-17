using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using CwkSocial.Domain.Validators.PostsValidators;

namespace CwkSocial.Domain.Aggregates.PostAggregate;

public class PostComment
{
    public Guid CommentId { get; private set; }

    public Guid PostId { get; private set; } // Foreign key to Post

    public Guid UserProfileId { get; private set; } // Foreign key to UserProfile

    // Marking Text as non-nullable and ensuring it's initialized to a non-null value.
    public string Text { get; private set; } = string.Empty;

    public DateTime CreatedDate { get; private set; }

    public DateTime LastModified { get; private set; }

    private PostComment() { }

    // Factory methods
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
