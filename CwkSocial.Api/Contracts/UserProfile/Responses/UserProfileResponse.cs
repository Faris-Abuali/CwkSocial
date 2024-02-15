namespace CwkSocial.Api.Contracts.UserProfile.Responses;

public record UserProfileResponse
{
    public Guid UserProfileId { get; init; }

    public BasicInformation? BasicInfo { get; init; }

    public DateTime CreatedDate { get; init; }

    public DateTime LastModified { get; init; }
}
