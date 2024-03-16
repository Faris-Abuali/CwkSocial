

using CwkSocial.DataAccess.Models;
using CwkSocial.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CwkSocial.Application.Identity.ConfirmEmail;

internal class ConfrimEmailCommadHandler
    : IRequestHandler<ConfirmEmailCommand, ErrorOr<Unit>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfrimEmailCommadHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<Unit>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Errors.Identity.UserNotFound;

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded)
            return Unit.Value;

        var errors = new List<Error>();

        // Convert IdentityResult to ErrorOr
        foreach (var error in result.Errors)
        {
            errors.Add(Error.Validation(error.Code, error.Description));
        }

        return errors;
    }
}
