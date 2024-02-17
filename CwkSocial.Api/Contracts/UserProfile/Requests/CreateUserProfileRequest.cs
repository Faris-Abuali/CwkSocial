using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.UserProfile.Requests;

public record CreateUserProfileRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string FirstName { get; init; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public required string LastName { get; init; }

    [Required]
    [EmailAddress]
    public string EmailAddress { get; init; } = string.Empty;

    public string? Phone { get; init; }

    [Required]
    public required DateTime DateOfBirth { get; init; }

    public string? CurrentCity { get; init; }
}
