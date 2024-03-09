using System.Reflection;

namespace CwkSocial.Api.Registrars;

public class AutoMapperRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
