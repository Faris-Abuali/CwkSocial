using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Post.Requests;

public record CreatePostRequest
{
    [Required]
    [MaxLength(500)]
    public required string TextContent { get; init; }
}
