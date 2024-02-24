namespace CwkSocial.Api.Contracts.Post.Responses;

public record ReactionAuthor
{
    public Guid UserProfileId { get; init; }
    public required string FullName { get; init; }
    public string? City { get; init; }
}
