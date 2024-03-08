using ErrorOr;
using FluentValidation;
using MediatR;

namespace CwkSocial.Application.Common.Behaviors;

public class FluentValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public FluentValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            // then the request has no validator
            // So, just delegate to the command/query handler
            return await next(); 
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next(); // delegate to the command/query handler
        }

        // Transform the errors to ErrorOr structure
        var errors = validationResult.Errors
            .ConvertAll(validationFailure => Error.Validation(
                code: validationFailure.PropertyName,
                description: validationFailure.ErrorMessage));

        return (dynamic)errors;
    }
}
