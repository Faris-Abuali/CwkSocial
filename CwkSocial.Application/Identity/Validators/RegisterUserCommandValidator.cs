using CwkSocial.Application.Identity.Commands;
using FluentValidation;

namespace CwkSocial.Application.Identity.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Password).MinimumLength(4).WithMessage("MMMM");
    }
}
