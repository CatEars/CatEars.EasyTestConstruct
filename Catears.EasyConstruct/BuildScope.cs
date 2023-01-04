using Catears.EasyConstruct.Registration;
using Catears.EasyConstruct.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct;

public class BuildScope
{
    internal ParameterResolverBundleCollection ResolverCollection { get; }
    
    protected IServiceCollection Collection { get; }

    private ServiceProvider? _provider;

    private ServiceProvider Provider
    {
        get { return _provider ??= Collection.BuildServiceProvider(); }
    }

    internal BuildScope(IServiceCollection serviceCollection, ParameterResolverBundleCollection resolverCollection)
    {
        Collection = serviceCollection;
        ResolverCollection = resolverCollection;
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

    internal virtual void InternalUse<T>(Type builtType, Func<IServiceProvider, T> builder) where T : class
    {
        var currentImplementation = Collection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (currentImplementation != null)
        {
            Collection.Remove(currentImplementation);
        }

        Collection.AddTransient(builtType, builder);
        InvalidateCurrentProvider();
    }

    public T Resolve<T>() where T : class
    {
        return (T)Resolve(typeof(T));
    }

    public object Resolve(Type type)
    {
        return InternalResolve(type);
    }

    public void Memoize<T>() where T : class
    {
        Memoize(typeof(T));
    }

    public void Memoize(Type type)
    {
        InternalMemoize(type);
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

    public BuildScope BindParameterFor<TService, TParam>(TParam value) 
        where TParam : class 
        where TService : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        var priorResolverBundle = GetParameterResolversForType<TService>();
        var constructor = priorResolverBundle.Constructor;
        var priorResolvers = priorResolverBundle.ParameterResolvers;
        var (_, priorIndex) = priorResolvers
            .Select((resolver, index) => (resolver, index))
            .First(resolverPair => resolverPair.resolver.Provides(typeof(TParam)));

        TService NewFactoryImplementation(IServiceProvider provider)
        {
            var parameters = new List<object>();
            for (var idx = 0; idx < priorResolvers.Count; ++idx)
            {
                var chosenParameter = idx == priorIndex
                    ? value
                    : priorResolvers[idx].ResolveParameter(provider);

                parameters.Add(chosenParameter);
            }

            return (TService) constructor.Invoke(parameters.ToArray());
        }

        InternalUse(typeof(TService), NewFactoryImplementation);
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