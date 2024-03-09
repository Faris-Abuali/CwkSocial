using CwkSocial.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CwkSocial.Application.DependencyInjection;

public static class FluentValidationConfig
{
    public static IServiceCollection AddFluentValidation(IServiceCollection services)
    {
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(MediatRValidationBehavior<,>));

        // Include validators from the application layer assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        /// Note:
        /// Thanks to the above code, otherwise, we had to add the validation 
        /// individually for each for each class like this: 👇😓

        //builder.Services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        
        return services;
    }
}
