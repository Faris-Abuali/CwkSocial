using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Post.Requests;

public record UpdatePostRequest
{

    [Required]
    [MaxLength(500)]
    [MinLength(3)]
    public required string TextContent { get; init; }
}
