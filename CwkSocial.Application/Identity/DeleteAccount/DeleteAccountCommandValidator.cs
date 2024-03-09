using FluentValidation;

namespace CwkSocial.Application.Identity.DeleteAccount;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountCommandValidator()
    {
        //RuleFor(x => x.IdentityUserId)
        //    .Must(userId => userId.ToString().StartsWith('5'))
        //    .WithMessage("IdentityUserId must start with '5'");
    }
}
