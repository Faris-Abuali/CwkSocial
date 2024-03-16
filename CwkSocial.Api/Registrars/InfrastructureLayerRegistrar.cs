
using CwkSocial.Infrastructure.DependencyInjection;

namespace CwkSocial.Api.Registrars;

public class InfrastructureLayerRegistrar : IWebApplicationBuilderRegistrar
{
    /// <summary>
    /// Extension method to register everything related to the infrastructure layer 
    /// in the DI container.
    /// <see cref="CwkSocial.Infrastructure"/>
    /// </summary>
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddInfrastructureLayer(builder.Configuration);
    }
} 

