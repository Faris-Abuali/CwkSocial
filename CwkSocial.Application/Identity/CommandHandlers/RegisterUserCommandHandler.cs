using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CwkSocial.Application.Identity.CommandHandlers;

internal class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public RegisterUserCommandHandler(
        DataContext context,
        UserManager<IdentityUser> userManager,
        IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<OperationResult<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            // Create a new IdentityUser
            await ValidateIdentityUser(result, request);

            if (result.IsError) return result;

            // -- Start a transaction to ensure that the user profile is created only if the user is created successfully --
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var identityUser = await CreateIdentityUserAsync(result, request, transaction, cancellationToken);

            if (result.IsError || identityUser is null) return result;

            var userProfile = await CreateUserProfileAsync(request, transaction, identityUser, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            // -- End of transaction --

            result.Payload = GetJwtString(identityUser, userProfile);
        }
        catch (UserProfileNotValidException ex)
        {
            ex.ValidationErrors
                .ForEach(msg => { result.AddError(msg); });
        }
        catch (Exception ex)
        {
            result.AddUnknownError(ex.Message);
        }

        return result;
    }

    private async Task ValidateIdentityUser(OperationResult<string> result, RegisterUserCommand request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.UserName);

        if (existingUser is not null)
            result.AddError(IdentityErrorMessages.IdentityUserAlreadyExist);
    }

    private async Task<IdentityUser?> CreateIdentityUserAsync(
        OperationResult<string> result,
        RegisterUserCommand request,
        IDbContextTransaction transaction,
        CancellationToken cancellationToken)
    {
        var identityUser = new IdentityUser
        {
            UserName = request.UserName,
            Email = request.UserName,
        };

        var createdIdentityUser = await _userManager.CreateAsync(identityUser, request.Password);

        //TODO: Add roles to the user
        //await _userManager.AddToRolesAsync(identityUser, ["X", "F"]);

        if (!createdIdentityUser.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            foreach (var error in createdIdentityUser.Errors)
            {
                result.AddError(error.Description, HttpStatusCode.BadRequest);
            }
        }

        return identityUser;
    }

    private async Task<UserProfile> CreateUserProfileAsync(
               RegisterUserCommand request,
               IDbContextTransaction transaction,
               IdentityUser identityUser,
               CancellationToken cancellationToken)
    {
        try
        {
            // Create BasiInfo instance
            var basicInfo = BasicInfo.Create(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Phone,
                request.DateOfBirth,
                request.CurrentCity);

            // Create UserProfile instance
            var userProfile = UserProfile.Create(identityUser.Id, basicInfo);

            // Add the user profile to the database
            _context.UserProfiles.Add(userProfile);

            await _context.SaveChangesAsync(cancellationToken);

            return userProfile;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
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
