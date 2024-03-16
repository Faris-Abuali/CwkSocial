using CwkSocial.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CwkSocial.Api.Contracts.Identity;

public record RegisterUserRequest
{
    [Required]
    [EmailAddress]
    public required string UserName { get; init; }

    [Required]
    [MinLength(6)]
    [MaxLength(150)]
    public required string Password { get; init; }

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    public required string FirstName { get; init; }

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    public required string LastName { get; init; }


    [Required]
    public DateTime DateOfBirth { get; init; }

    [Phone]
    public string? Phone { get; init; }

    [MaxLength(50)]
    [MinLength(2)]
    public required string CurrentCity { get; init; }

    public string[] Roles { get; init; } = [];
}
