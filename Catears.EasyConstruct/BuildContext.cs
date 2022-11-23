using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Registrators;
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

    public void Register<T>() where T : class
    {
        var registrationContext = ServiceRegistrationContext.FromType(typeof(T));
        DefaultServiceRegistratorChain.FirstLink.TryRegisterService(ServiceCollection, registrationContext);
    }

    public void Register<T>(Func<IServiceProvider, T> builder) where T : class
    {
        ServiceCollection.AddTransient(builder);
    }

    public void Register<T>(Func<T> builder) where T : class
    {
        Register(_ => builder());
    }

    public IBuildScope Scope()
    {
        return new BuildScope(CopyOf(ServiceCollection));
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}