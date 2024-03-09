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

        public static Error IdentityUserAlreadyExist => Error.Conflict(
            code: "Identity.IdentityUserAlreadyExist",
            description: "Provided email address already exists. Can't register new user");
        
        public static Error NotIdentityOwner(string accountId) => Error.Forbidden(
            code: "Identity.NotIdentityOwner",
            description: $"You are not the owner of the account with id: {accountId}");

        public static Error FailedToCreateIdentityUser => Error.Failure(
            code: "Identity.FailedToCreateIdentityUser",
            description: "Something went wrong while creating identity user");
    }
}
