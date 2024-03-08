

using ErrorOr;

namespace CwkSocial.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error InvalidProfile => Error.Validation(
            code: "User.InvalidProfile",
            description: "The basic info is not valid.");

        public static Error UserProfileNotFound => Error.NotFound(
            code: "User.UserProfileNotFound",
            description: "User profile not found");
    }
}
