using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catears.EasyConstruct;

public class BuildContext
{

    public class Options
    {
        public RegistrationMode RegistrationMode { get; set; } = RegistrationMode.Basic;

        public Func<Type, object>? MockFactoryMethod { get; set; }

        public static Options Default { get; } = new();
    }

    private ServiceCollection ServiceCollection { get; } = new();

    private Options BuildOptions { get; }

    public BuildContext(Options? options = null)
    {
        BuildOptions = options ?? Options.Default;
        ServiceCollection.RegisterBasicValueProviders();
    }

    public void Register(Type type)
    {
        var registrator = new ServiceRegistrator(BuildOptions.MockFactoryMethod);
        var dependencyWalker = GetDependencyWalkerForType(type);
        registrator.RegisterServicesOrThrow(ServiceCollection, dependencyWalker);
    }

    private IServiceDependencyWalker GetDependencyWalkerForType(Type type)
    {
        return BuildOptions.RegistrationMode == RegistrationMode.Recursive
            ? new RecursiveServiceDependencyWalker(type)
            : new BasicServiceDependencyWalker(type);
    }

    public void Register<T>() where T : class
    {
        Register(typeof(T));
    }

    public void Register(Type type, Func<IServiceProvider, object> builder)
    {
        ServiceCollection.AddTransient(type, builder);
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