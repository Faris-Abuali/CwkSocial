using CwkSocial.Application.Models;
using CwkSocial.Application.Services;
using CwkSocial.DataAccess;
using CwkSocial.DataAccess.Models;
using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Domain.Common.Errors;
using CwkSocial.Domain.Services;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly IEmailService _emailService;

    public RegisterUserCommandHandler(
        DataContext context,
        UserManager<ApplicationUser> userManager,
        IdentityService identityService,
        IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
        _emailService = emailService;
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

            var registerToken = GetJwtString(identityUser, userProfile);

            // --- Email Confirmation ---
            await _identityService.GenerateAndSendEmailConfirmationToken(request.ConfirmationLink, identityUser);

            return registerToken;
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

    private async Task SendEmailConfirmationToken(string url, ApplicationUser identityUser)
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

    private async Task<ErrorOr<bool>> ValidateIdentityUser(RegisterUserCommand request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.UserName);

        if (existingUser is not null)
            return Errors.Identity.IdentityUserAlreadyExist;

        return true;
    }

    private async Task<ErrorOr<ApplicationUser?>> CreateIdentityUserAsync(
        RegisterUserCommand request,
        IDbContextTransaction transaction,
        CancellationToken cancellationToken)
    {
        var appUserResult = ApplicationUser.Create(
            request.UserName,
            request.UserName,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.CurrentCity);

        if (appUserResult.IsError)
            return appUserResult.Errors;

        var appUser = appUserResult.Value;

        var createdIdentityUser = await _userManager.CreateAsync(appUser, request.Password);

        // Convert request.Roles to string list
        var roles = request.Roles.Select(role => role.ToString()).ToList();

        await _userManager.AddToRolesAsync(appUser, roles);

        if (!createdIdentityUser.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);

            // Convert the IdentityError to ErrorOr.Error
            var errors = createdIdentityUser.Errors
                .Select(error => ErrorOr.Error.Validation(error.Code, error.Description))
                .ToList();

            return errors!;
            //return ErrorOr<ApplicationUser?>.From(errors);
        }

        return appUser;
    }

    private async Task<ErrorOr<UserProfile>> CreateUserProfileAsync(
               RegisterUserCommand request,
               IDbContextTransaction transaction,
               ApplicationUser identityUser,
               CancellationToken cancellationToken)
    {
        try
        {
            // Create BasiInfo instance
            var basicInfo = BasicInfo.Create(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.PhoneNumber,
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

    private string GetJwtString(ApplicationUser identityUser, UserProfile userProfile)
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
