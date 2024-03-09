
using CwkSocial.Application.DependencyInjection;

namespace CwkSocial.Api.Registrars;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    /// <summary>
    /// Extension method to register everything related to the application layer 
    /// in the DI container.
    /// <see cref="CwkSocial.Application"/>
    /// </summary>
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationLayer();
    }
}
