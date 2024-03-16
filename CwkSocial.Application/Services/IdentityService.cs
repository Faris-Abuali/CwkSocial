using CwkSocial.Application.Options;
using CwkSocial.DataAccess.Models;
using CwkSocial.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CwkSocial.Application.Services;

public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;
    private UserManager<ApplicationUser> _userManager;
    private IEmailService _emailService;

    public IdentityService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService)
    {
        _jwtSettings = jwtSettings.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
        _userManager = userManager;
        _emailService = emailService;
    }

    public JwtSecurityTokenHandler TokenHandler = new();

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        // Create & return a JWT token
        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddDays(7),
            Audience = _jwtSettings.Audiences.First(),
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(
                           key: new SymmetricSecurityKey(_key),
                           SecurityAlgorithms.HmacSha256Signature),
        };
    }


    public async Task GenerateAndSendEmailConfirmationToken(string url, ApplicationUser identityUser)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

        // Encode the token to be used as a query param in the confirmation link
        var endocedConfirmationToken = WebUtility.UrlEncode(confirmationToken);

        // Add the token as query param to the confirmation link
        var emailConfirmationUrl = $"{url}&token={endocedConfirmationToken}";

        // Send an email to the user to verify their email address
        await _emailService.SendEmailConfirmationTokenAsync(
                           identityUser.Email!,
                           emailConfirmationUrl);
    }
}
