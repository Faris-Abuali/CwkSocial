using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.Application.Identity.CommandHandlers;

internal class LoginCommandHandler
    : IRequestHandler<LoginCommand, OperationResult<string>>
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

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var identityUser = await ValidateAndGetIdentityAsync(request, result);

            if (identityUser is null) return result;

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id);

            if (userProfile is null)
            {
                result.Errors = [
                    new Error
                    {
                        Message = "User profile not found"
                    }
                ];
                return result;
            }

            result.Payload = GetJwtString(identityUser, userProfile);
        }
        catch (Exception ex)
        {
            result.Errors = [new Error{
                Message = ex.Message,
            }];
        }

        return result;
    }


    private async Task<IdentityUser?> ValidateAndGetIdentityAsync(
        LoginCommand request,
        OperationResult<string> result)
    {
        var identityUser = await _userManager.FindByNameAsync(request.UserName);

        if (identityUser is null)
        {
            result.Errors = [
                new Error
                    {
                        Message = $"No such user found with user name {request.UserName}"
                    }
            ];
            return null;
        }

        var passwordValid = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!passwordValid)
        {
            result.Errors = [
                new Error
                    {
                        Message = "Invalid login credentials"
                    }
            ];
            return null;
        }

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
