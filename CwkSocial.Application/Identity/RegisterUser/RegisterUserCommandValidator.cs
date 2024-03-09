using FluentValidation;

namespace CwkSocial.Application.Identity.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Password).MinimumLength(4).WithMessage("MMMM");
    }
}
