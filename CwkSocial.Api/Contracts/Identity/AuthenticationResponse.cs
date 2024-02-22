namespace CwkSocial.Api.Contracts.Identity;

public record AuthenticationResponse
{
    public required string Token { get; init; }
}
