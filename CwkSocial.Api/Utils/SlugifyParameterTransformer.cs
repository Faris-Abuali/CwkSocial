using System.Text.RegularExpressions;

namespace CwkSocial.Api.Utils;

public sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) { return null; }
        string? str = value.ToString();
        if (string.IsNullOrEmpty(str)) { return null; }

        /**
         * replaces every occurrence of a lowercase letter followed by an 
         * uppercase letter in the input string (str) with:
         * the lowercase letter, a hyphen, and the uppercase letter.
         * e.g. "HelloWorld" -> "hello-world"
         */
        return Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
