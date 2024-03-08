using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Identity;

public class LoginRequest
{
    //[Required]
    //[EmailAddress]
    public required string UserName { get; init; }

    //[Required]
    //[MinLength(6)]
    //[MaxLength(150)]
    public required string Password { get; init; }
}
