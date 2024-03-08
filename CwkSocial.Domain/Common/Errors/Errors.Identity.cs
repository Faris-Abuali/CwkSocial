using ErrorOr;

namespace CwkSocial.Domain.Common.Errors;

public static partial class Errors
{
    public static class Identity
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Identity.InvalidCredentials",
            description: "Invalid credentials");

        public static Error NonExistentIdentityUser => Error.NotFound(
            code: "Identity.NonExistentIdentityUser",
            description: "No such user found with the specified username");
    }
}
