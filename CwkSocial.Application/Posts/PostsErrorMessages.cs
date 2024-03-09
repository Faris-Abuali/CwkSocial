namespace CwkSocial.Application.Posts;

[Obsolete("This is deprecated, use `CwkSocial.Domain.Common.Errors` instead")]
public class PostsErrorMessages
{
    public const string PostNotFound = "No post found with ID: {0}";
    public const string NotPostOwner = "You are not the owner of the post";
    public const string ReactionNotFound = "No reaction found with ID: {0}";
    public const string NotReactionOwner = "You are not the owner of the reaction";
}
