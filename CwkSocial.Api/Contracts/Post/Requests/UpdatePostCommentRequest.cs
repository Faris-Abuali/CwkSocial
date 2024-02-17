namespace CwkSocial.Api.Contracts.Post.Requests;

public record UpdatePostCommentRequest
{
    public required string Text { get; init; }
}
