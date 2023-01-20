using CatEars.HappyBuild.DependencyListers;
using CatEars.HappyBuild.MockFactories;
using CatEars.HappyBuild.Providers;
using CatEars.HappyBuild.Registration;
using CatEars.HappyBuild.Scopes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CatEars.HappyBuild;

public class BuildContext
{
    public class Options
    {
        public RegistrationMode RegistrationMode { get; set; } = RegistrationMode.Dynamic;

        public MockFactory MockFactory { get; set; } = new ThrowingMockFactory();

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

    private IDependencyLister GetConfiguredDependencyWalker()
    {
        return BuildOptions.RegistrationMode == RegistrationMode.Dynamic
            ? new RecursiveDependencyListerDecorator(new ConstructorParameterDependencyLister())
            : new BasicDependencyLister();
    }

    public void Register<T>() where T : class
    {
        var registrator = new ServiceRegistrator(BuildOptions.MockFactory, ResolverCollection);
        var dependencyWalker = GetConfiguredDependencyWalker();
        registrator.RegisterServicesOrThrow(ServiceCollection, dependencyWalker, typeof(T));
    }

    public void Register<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        Register<TService>(provider => provider.GetRequiredService<TImplementation>());
        ServiceCollection.AddTransient<TImplementation>();
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
            ? new DynamicScope(CopyOf(ServiceCollection), ResolverCollection.Copy(), BuildOptions.MockFactory)
            : new BuildScopeImpl(CopyOf(ServiceCollection), ResolverCollection.Copy());
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}