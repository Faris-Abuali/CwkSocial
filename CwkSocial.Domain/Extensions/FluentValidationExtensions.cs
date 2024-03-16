using ErrorOr;
using FluentValidation;

namespace CwkSocial.Domain.Extensions;

public static class FluentValidationExtensions
{
    public static ErrorOr<T> ValidateAndConvertToErrorOr<T>(this AbstractValidator<T> validator, T instance)
    {
        var validationResult = validator.Validate(instance);

        if (validationResult.IsValid)
            return instance;


        var errors = new List<Error>();

        foreach (var error in validationResult.Errors)
        {
            errors.Add(Error.Validation(error.PropertyName, error.ErrorMessage));
        }

        //return ErrorOr<T>.From(errors);
        return errors;
    }
}
