using CwkSocial.Domain.Aggregates.UserProfileAggregate;
using FluentValidation;

namespace CwkSocial.Domain.Validators.UserProfileValidators;

public class BasicInfoValidator : AbstractValidator<BasicInfo>
{
    public BasicInfoValidator()
    {
        RuleFor(basicInfo => basicInfo.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters.")
            .Matches(@"^[a-zA-Z\s]*$").WithMessage("First name must contain only letters and spaces.")
            .Must((basicInfo, firstName) => basicInfo.LastName != basicInfo.FirstName)
            .WithMessage("First name and last name cannot be the same.");

        RuleFor(basicInfo => basicInfo.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
            .Matches(@"^[a-zA-Z\s]*$").WithMessage("Last name must contain only letters and spaces.");

        RuleFor(basicInfo => basicInfo.EmailAddress)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is not valid.")
            .MaximumLength(50).WithMessage("Email address must not exceed 50 characters.");

        RuleFor(basicInfo => basicInfo.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");

        RuleFor(basicInfo => basicInfo.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.");


        RuleFor(basicInfo => basicInfo.DateOfBirth.Year)
            .InclusiveBetween(DateTime.Now.Year - 125, DateTime.Now.Year - 18)
            .WithMessage("Age must be between 18 and 125 years");


        RuleFor(basicInfo => basicInfo.CurrentCity)
            .NotEmpty().WithMessage("Current city is required.")
            .MaximumLength(50).WithMessage("Current city must not exceed 50 characters.")
            .MinimumLength(2).WithMessage("Current city must be at least 2 characters.")
            .Matches(@"^[a-zA-Z\s]*$").WithMessage("Current city must contain only letters and spaces.");
    }
}
