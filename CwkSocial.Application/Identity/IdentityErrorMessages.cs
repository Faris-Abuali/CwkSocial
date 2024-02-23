
using Azure.Core;

namespace CwkSocial.Application.Identity;

public class IdentityErrorMessages
{
    public const string NonExistentIdentityUser = "No such user found with the specified usrname";
    public const string InvalidLoginCredentials = "Invalid login credentials";
    public const string IdentityUserAlreadyExist = "Provided email address already exists. Can't register new user";

}
