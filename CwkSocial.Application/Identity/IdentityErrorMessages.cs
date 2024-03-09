
namespace CwkSocial.Application.Identity;

[Obsolete("This is deprecated, use `CwkSocial.Domain.Common.Errors` instead")]
public class IdentityErrorMessages
{
    public const string NonExistentIdentityUser = "No such user found with the specified username";
    public const string InvalidLoginCredentials = "Invalid login credentials";
    public const string IdentityUserAlreadyExist = "Provided email address already exists. Can't register new user";
    public const string IdentityUserWithNoUserProfile = "No such user found with the specified identity id: {0}";
    public const string NotIdentityOwner = "You are not the owner of the account with id: {0}";
}
