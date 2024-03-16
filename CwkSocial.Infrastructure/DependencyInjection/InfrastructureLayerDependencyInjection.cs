
using CwkSocial.Domain.Services;
using CwkSocial.Infrastructure.Options;
using CwkSocial.Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CwkSocial.Infrastructure.DependencyInjection;

/// <summary>
/// Register everything related to the infrastructure layer in the DI container.
/// </summary>
public static class InfrastructureLayerDependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Bind EmailConfiguration section from appsettings.json to EmailConfiguration class
        var emailConfig = new EmailConfiguration();
        configuration.Bind(nameof(EmailConfiguration), emailConfig);

        // Now configure the JwtSettings class to be injected into the Dependency Injection container
        var emailSection = configuration.GetSection(nameof(EmailConfiguration));
        services.Configure<EmailConfiguration>(emailSection);

        //// Register emailConfig instance in the DI container
        //services.AddSingleton(emailConfig);

        // Add the email service to the DI container
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
