using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace CwkSocial.Application.Identity.RegisterUser;

internal class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, ErrorOr<string>>
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

    public async Task<ErrorOr<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create a new IdentityUser
            var result = await ValidateIdentityUser(request);

            if (result.IsError) return result.Errors;

            // -- Start a transaction to ensure that the user profile is created only if the user is created successfully --
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var createIdentityUserResult = await CreateIdentityUserAsync(request, transaction, cancellationToken);

            if (createIdentityUserResult.IsError) return createIdentityUserResult.Errors;

            var identityUser = createIdentityUserResult.Value;

            if (identityUser is null)
                return Errors.Identity.FailedToCreateIdentityUser;

            var createUserProfileResult = await CreateUserProfileAsync(request, transaction, identityUser, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            // -- End of transaction --

            if (createUserProfileResult.IsError)
                return createUserProfileResult.Errors;

            var userProfile = createUserProfileResult.Value;

            return GetJwtString(identityUser, userProfile);
        }
        //catch (UserProfileNotValidException ex)
        //{
        //    ex.ValidationErrors
        //        .ForEach(msg => { result.AddError(msg); });
        //}
        catch (Exception ex)
        {
            return Errors.Unknown.Create(ex.Message);
        }
    }

    private async Task<ErrorOr<bool>> ValidateIdentityUser(RegisterUserCommand request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.UserName);

        if (existingUser is not null)
            return Errors.Identity.IdentityUserAlreadyExist;

        return true;
    }

    private async Task<ErrorOr<IdentityUser?>> CreateIdentityUserAsync(
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

            // Convert the IdentityError to ErrorOr.Error
            var errors = createdIdentityUser.Errors
                .Select(error => ErrorOr.Error.Validation(error.Code, error.Description))
                .ToList();

            return ErrorOr<IdentityUser?>.From(errors);
        }

        return identityUser;
    }

    private async Task<ErrorOr<UserProfile>> CreateUserProfileAsync(
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

            if (basicInfo.IsError)
                return basicInfo.Errors;

            // Create UserProfile instance
            var userProfile = UserProfile.Create(identityUser.Id, basicInfo.Value);

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
