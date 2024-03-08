using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.Application.Identity.CommandHandlers;

internal class LoginCommandHandler
    : IRequestHandler<LoginCommand, ErrorOr<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(
        DataContext context,
        UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAndGetIdentityAsync(request);

        if (validationResult.IsError)
            return validationResult.Errors;

        // If there's no error so far, then the validationResult.Value holds the identityUser
        var identityUser = validationResult.Value;

        // Find the user profile linked with this identityUser
        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id);

        if (userProfile is null)
            return Errors.User.UserProfileNotFound;

        return GetJwtString(identityUser, userProfile);

    }

    private async Task<ErrorOr<IdentityUser>> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        var identityUser = await _userManager.FindByNameAsync(request.UserName);

        if (identityUser is null)
            return Errors.Identity.NonExistentIdentityUser;

        var passwordValid = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!passwordValid)
            return Errors.Identity.InvalidCredentials;

        return identityUser;
    }

    private string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new[]
              {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email!),
                    new Claim("IdentityId", identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString()),
                });

        // Create a JWT token
        var token = _identityService.CreateSecurityToken(claimsIdentity);

        return _identityService.WriteToken(token);
    }
}
