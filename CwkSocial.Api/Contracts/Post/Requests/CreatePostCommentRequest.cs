using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Post.Requests;

public record CreatePostCommentRequest
{
    [Required]
    [MaxLength(500)]
    public required string Text { get; init; }
}
