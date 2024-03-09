
using CwkSocial.Application.Identity.Commands;
using FluentValidation;

namespace CwkSocial.Application.Identity.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Password).MinimumLength(3).WithMessage("Just testing :)");
    }
}
