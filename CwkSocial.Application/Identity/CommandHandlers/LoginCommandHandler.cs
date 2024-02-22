using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
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
            var identityUser = await _userManager.FindByNameAsync(request.UserName);

            if (identityUser is null)
            {
                result.Errors = [
                    new Error
                    {
                        Message = $"No such user found with user name {request.UserName}"
                    }
                ];
                return result;
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
                return result;
            }

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

            result.Payload = _identityService.WriteToken(token);
        }
        catch (Exception ex)
        {
            result.Errors = [new Error{
                Message = ex.Message,
            }];
        }

        return result;
    }
}
