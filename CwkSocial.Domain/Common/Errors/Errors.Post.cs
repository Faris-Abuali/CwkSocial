

using ErrorOr;

namespace CwkSocial.Domain.Common.Errors;

public static partial class Errors
{
    public static class Post
    {
        //public static Error InvalidProfile => Error.Validation(
        //    code: "User.InvalidProfile",
        //    description: "The basic info is not valid.");

        public static Error PostNotFound => Error.NotFound(
            code: "Post.PostNotFound",
            description: "Post not found");

        public static Error NotPostOwner(string postId) => Error.Forbidden(
            code: "Identity.NotIdentityOwner",
            description: $"You are not the owner of the post of id: {postId}");

        public static Error ReactionNotFound => Error.NotFound(
            code: "Post.ReactionNotFound",
            description: "Reaction not found");

        public static Error NotReactionOwner(string reactionId) => Error.Forbidden(
            code: "Identity.NotReactionOwner",
            description: $"You are not the owner of the reaction of id: {reactionId}");
    }
}
