﻿
namespace CwkSocial.Api;

public class ApiRoutes
{
    public const string BaseRoute = "api/v{version:apiVersion}/[controller]";

    public static class UserProfiles
    {
        public const string IdRoute = "{id}";
    }

    public static class Posts
    {
        public const string IdRoute = "{id}";
        public const string PostComments = "{postId}/comments";
        public const string CommentById = "{postId}/comments/{commentId}";
        public const string PostReactions = "{postId}/reactions";
        public const string ReactionById = "{postId}/reactions/{reactionId}";
    }

    public static class Identity
    {
        public const string Login = "login";
        public const string Registration = "registration";
        public const string IdentityById = "{identityUserId}";
        public const string ConfirmEmail = "email-confirmation";
    }
}
