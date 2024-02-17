namespace CwkSocial.Api.Contracts.Post.Responses;

public record PostCommentResponse
{
    public required string PostId { get; init; }
    public required string Text { get; init; }
    public required string UserProfileId { get; init; }
    public required string CommentId { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime LastModified { get; init; }
}
