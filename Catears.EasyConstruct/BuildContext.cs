using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catears.EasyConstruct;

public class BuildContext
{
    private ServiceCollection ServiceCollection { get; } = new();

    public BuildContext()
    {
        ServiceCollection.RegisterBasicValueProviders();
    }

    public void Register(Type type)
    {
        var registrationContext = ServiceRegistrationContext.FromType(type);
        ServiceRegistrator.RegisterServiceOrThrow(ServiceCollection, registrationContext);
    }
    
    public void Register<T>() where T : class
    {
        Register(typeof(T));
    }

    public void Register<T>(Func<IServiceProvider, T> builder) where T : class
    {
        ServiceCollection.AddTransient(builder);
    }

    public void Register<T>(Func<T> builder) where T : class
    {
        Register(_ => builder());
    }

    public BuildScope Scope()
    {
        return new BuildScope(CopyOf(ServiceCollection));
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}