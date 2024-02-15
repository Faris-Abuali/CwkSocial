namespace CwkSocial.Api.Contracts.UserProfile.Responses;

public record BasicInformation
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string EmailAddress { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public DateTime DateOfBirth { get; init; }

    public string? CurrentCity { get; init; }
}
