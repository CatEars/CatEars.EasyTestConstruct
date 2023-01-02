using Catears.EasyConstruct.DependencyListers;
using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Registration;
using Catears.EasyConstruct.Scopes;
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

    private ParameterResolverBundleCollection ResolverCollection { get; } =
        new(new Dictionary<Type, ParameterResolverBundle>());
    
    public BuildContext(Options? options = null)
    {
        BuildOptions = options ?? Options.Default;
        ServiceCollection.RegisterBasicValueProviders();
    }

    public void Register(Type type)
    {
        var registrator = new ServiceRegistrator(BuildOptions.MockFactoryMethod, ResolverCollection);
        var dependencyWalker = GetConfiguredDependencyWalker();
        registrator.RegisterServicesOrThrow(ServiceCollection, dependencyWalker, type);
    }

    private IDependencyLister GetConfiguredDependencyWalker()
    {
        return BuildOptions.RegistrationMode == RegistrationMode.Dynamic
            ? new RecursiveDependencyListerDecorator(new ConstructorParameterDependencyLister())
            : new BasicDependencyLister();
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
        return BuildOptions.RegistrationMode == RegistrationMode.Dynamic
            ? new DynamicScope(CopyOf(ServiceCollection), ResolverCollection.Copy(), BuildOptions.MockFactoryMethod)
            : new BuildScope(CopyOf(ServiceCollection), ResolverCollection.Copy());
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}