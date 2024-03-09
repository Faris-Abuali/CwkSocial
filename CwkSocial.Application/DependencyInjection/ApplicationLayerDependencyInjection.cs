using CwkSocial.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CwkSocial.Application.DependencyInjection;

/// <summary>
/// Register everything related to the application layer in the DI container.
/// </summary>
public static class ApplicationLayerDependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        // 1. Fluent Validation
        FluentValidationConfig.AddFluentValidation(services);

        // 2. AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // 3. Services
        services.AddScoped<IdentityService>();

        return services;
    }
}
