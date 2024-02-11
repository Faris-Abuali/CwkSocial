using CwkSocial.Api.Registrars;

namespace CwkSocial.Api.Extensions;

public static class RegistrarExtensions
{
    // Extension method to register services in the WebApplicationBuilder
    public static void RegisterServices(this WebApplicationBuilder builder, Type scanningType)
    {
        var registrars = GetRegistrars<IWebApplicationBuilderRegistrar>(scanningType);

        foreach (var registrar in registrars)
        {
            registrar.RegisterServices(builder);
        }
    }

    // Extension method to register pipeline components in the WebApplication
    public static void RegisterPipelineComponents(this WebApplication app, Type scanningType)
    {
        var registrars = GetRegistrars<IWebApplicationRegistrar>(scanningType);

        foreach (var registrar in registrars)
        {
            registrar.RegisterPipelineComponents(app);
        }
    }

    /// <summary>
    /// Get all classes that implement interface T
    /// Create an instance of each class
    /// Cast each instance to interface T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scanningType"></param>
    /// <returns></returns> <summary>
    private static IEnumerable<T> GetRegistrars<T>(Type scanningType) where T : IRegistrar
    {
        return scanningType.Assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(T)) && !t.IsAbstract && !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<T>();
    }
}
