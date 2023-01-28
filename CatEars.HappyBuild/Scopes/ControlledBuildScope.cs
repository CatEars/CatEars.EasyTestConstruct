using CatEars.HappyBuild.Registration;
using CatEars.HappyBuild.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Scopes;

public class ControlledBuildScope : BuildScope
{
    internal ParameterResolverBundleCollection ResolverCollection { get; }

    protected IServiceCollection Collection { get; }

    private ServiceProvider? _provider;

    public BuildContext.Options Options { get; }
    
    private ServiceProvider Provider
    {
        get { return _provider ??= Collection.BuildServiceProvider(); }
    }

    internal ControlledBuildScope(IServiceCollection serviceCollection,
        ParameterResolverBundleCollection resolverCollection,
        BuildContext.Options options)
    {
        Collection = serviceCollection;
        ResolverCollection = resolverCollection;
        Options = options;
    }

    internal virtual object InternalResolve(Type type)
    {
        return Provider.GetRequiredService(type);
    }

    internal virtual void InternalMemoize(Type type)
    {
        var currentImplementation = Collection.First(descriptor => descriptor.ServiceType == type);
        var priorFactory = currentImplementation.ImplementationFactory;
        if (priorFactory == null)
        {
            if (currentImplementation.ImplementationInstance == null)
            {
                throw new InvalidOperationException();
            }

            priorFactory = _ => currentImplementation.ImplementationInstance;
        }

        var resolver = new MemoizedResolver(provider => priorFactory(provider), type);
        InternalUse(type, provider => resolver.ResolveParameter(provider));
    }

    private void InternalUse<T>(Type builtType, Func<IServiceProvider, T> builder) where T : class
    {
        var currentImplementation = Collection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (currentImplementation != null)
        {
            Collection.Remove(currentImplementation);
        }

        Collection.AddTransient(builtType, builder);
        InvalidateCurrentProvider();
    }

    public T Build<T>() where T : class
    {
        return (T)InternalResolve(typeof(T));
    }

    public void Memoize<T>() where T : class
    {
        InternalMemoize(typeof(T));
    }

    public void Use<T>(Func<IServiceProvider, T> builder) where T : class
    {
        InternalUse(typeof(T), builder);
    }

    public void Use<T>(Func<T> builder) where T : class
    {
        InternalUse(typeof(T), _ => builder());
    }

    protected void InvalidateCurrentProvider()
    {
        _provider = null;
    }

    public BuildScope BindParameter<TService, TParam>(TParam value)
        where TService : class
    {
        return BindNthParameterOfType<TService, TParam>(value, 0);
    }

    public BuildScope BindNthParameterOfType<TService, TParam>(TParam value, int parameterIndex)
        where TService : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var priorResolverBundle = GetParameterResolversForType<TService>();
        var priorResolvers = priorResolverBundle.ParameterResolvers;
        var chosenIndex = priorResolvers
            .Select((resolver, index) => (resolver, index))
            .First(x => x.resolver.Provides(typeof(TParam)) && parameterIndex-- == 0)
            .index;
        return BindNthParameter(value, chosenIndex, priorResolverBundle);
    }

    public BuildScope BindNthParameter<TService, TParam>(TParam value, int index) 
        where TService : class
    {
        var priorResolverBundle = GetParameterResolversForType<TService>();
        return BindNthParameter(value, index, priorResolverBundle);
    }

    private BuildScope BindNthParameter<TParam>(
        TParam value, 
        int index, 
        ParameterResolverBundle bundle)
    {
        var newResolver = new FuncResolver(_ => value, typeof(TParam));
        bundle.ParameterResolvers[index] = newResolver;
        return this;
    }

    private ParameterResolverBundle GetParameterResolversForType<TService>()
    {
        var type = typeof(TService);
        if (ResolverCollection.BundleMap.TryGetValue(type, out var bundle))
        {
            return bundle;
        }

        var msg = $"Tried to bind parameter for '{type.Name}', but no such type seems to be registered";
        throw new InvalidOperationException(msg);
    }
}