using ErrorOr;
using System.Diagnostics;

namespace CwkSocial.Domain.Common.Errors;

public static partial class Errors
{
    public static class Identity
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Identity.InvalidCredentials",
            description: "Invalid credentials");

        public static Error UserNotFound => Error.NotFound(
            code: "Identity.UserNotFound",
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

        public static Error EmailNotConfirmed => Error.Forbidden(
            code: "Identity.EmailNotConfirmed",
            description: "Email is not confirmed");

        public static Error InvalidUserName => Error.Validation(
            code: "Identity.InvalidUserName",
            description: "Invalid username");
    }
}
