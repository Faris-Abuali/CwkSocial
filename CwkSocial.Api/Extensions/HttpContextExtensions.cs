using System.Security.Claims;

namespace CwkSocial.Api.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("UserProfileId", context);
    }

    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        return GetGuidClaimValue("IdentityId", context);
    }

    /// <summary>
    /// Get the value of a claim and convert it to a Guid
    /// </summary>
    /// <param name="key">The claim type</param>
    /// <param name="context">The HttpContext</param>
    /// <returns>The value of a claim as a Guid</returns>
    private static Guid GetGuidClaimValue(string key, HttpContext context)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var stringValue = identity?.FindFirst(key)?.Value;
        return Guid.Parse(stringValue!);

    }
}
