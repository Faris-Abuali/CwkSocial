namespace CwkSocial.Api.Contracts.UserProfile.Responses;

public record PostResponse
{
    public Guid PostId { get; init; }
    public Guid UserProfileId { get; init; }
    public string TextContent { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime LastModified { get; init; }
}
