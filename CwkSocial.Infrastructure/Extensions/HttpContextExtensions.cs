using Microsoft.AspNetCore.Http;

namespace CwkSocial.Infrastructure.Extensions;

public static class HttpContextExtensions
{
    public static string GetRequestBaseUrl(this HttpContext context)
    {
        var host = context.Request.Host.Value;
        var scheme = context.Request.Scheme;

        return $"{scheme}://{host}";
    }
}
