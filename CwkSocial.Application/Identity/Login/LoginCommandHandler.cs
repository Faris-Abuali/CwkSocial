using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.DataAccess.Models;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityService _identityService;

    public LoginCommandHandler(
        DataContext context,
        UserManager<ApplicationUser> userManager,
        IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<ErrorOr<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await ValidateAndGetIdentityAsync(request);

            if (validationResult.IsError)
                return validationResult.Errors;

            // If there's no error so far, then the validationResult.Value holds the ApplicationUser
            var ApplicationUser = validationResult.Value;

            // If email is not confirmed, return an error
            if (!ApplicationUser.EmailConfirmed)
                return Errors.Identity.EmailNotConfirmed;

            // Find the user profile linked with this ApplicationUser
            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == ApplicationUser.Id);

            if (userProfile is null)
                return Errors.User.UserProfileNotFound;

            return GetJwtString(ApplicationUser, userProfile);
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }

    private async Task<ErrorOr<ApplicationUser>> ValidateAndGetIdentityAsync(LoginCommand request)
    {
        try
        {
            var ApplicationUser = await _userManager.FindByNameAsync(request.UserName);

            if (ApplicationUser is null)
                return Errors.Identity.UserNotFound;

            var passwordValid = await _userManager.CheckPasswordAsync(ApplicationUser, request.Password);

            if (!passwordValid)
                return Errors.Identity.InvalidCredentials;

            return ApplicationUser;
        }
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }

    private string GetJwtString(ApplicationUser ApplicationUser, UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new[]
              {
                    new Claim(JwtRegisteredClaimNames.Sub, ApplicationUser.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, ApplicationUser.Email!),
                    new Claim("IdentityId", ApplicationUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString()),
                });

        // Create a JWT token
        var token = _identityService.CreateSecurityToken(claimsIdentity);

        return _identityService.WriteToken(token);
    }
}
