
using CwkSocial.Application.Common.Behaviors;
using CwkSocial.Application.UserProfiles.GetAllUserProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;

namespace CwkSocial.Api.Registrars;

public class FluentValidationRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // --- API LAYER ---
        // Add the `FluentValidation package` services to the DI container
        builder.Services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        // Get all types in the assembly that implement AbstractValidator
        var validatorTypes = Assembly
            .GetCallingAssembly()
            .GetTypes()
            .Where(t => t.BaseType != null 
                        && t.BaseType.IsGenericType 
                        && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

        // Register each validator
        foreach (var validatorType in validatorTypes)
        {
            // Get the generic argument of AbstractValidator<>
            var targetType = validatorType.BaseType?.GetGenericArguments().FirstOrDefault();
            if (targetType != null)
            {
                // Register the validator with the specific type it validates
                builder.Services
                    .AddScoped(typeof(IValidator<>)
                    .MakeGenericType(targetType), validatorType);
            }
        }
    }
}
