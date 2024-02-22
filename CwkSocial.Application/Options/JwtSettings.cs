
namespace CwkSocial.Application.Options;

public class JwtSettings
{
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string[] Audiences { get; set; } = [];
}
