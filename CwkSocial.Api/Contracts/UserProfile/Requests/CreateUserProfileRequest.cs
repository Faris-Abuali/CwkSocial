namespace CwkSocial.Api.Contracts.UserProfile.Requests;

public record CreateUserProfileRequest
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string? CurrentCity { get; init; }
}
