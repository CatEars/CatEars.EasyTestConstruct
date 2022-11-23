using Catears.EasyConstruct.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct;

internal class BuildScope : IBuildScope
{
    private IServiceCollection Collection { get; }

    private ServiceProvider? _provider = null;

    private ServiceProvider Provider
    {
        get { return _provider ??= Collection.BuildServiceProvider(); }
    }

    public BuildScope(IServiceCollection serviceCollection)
    {
        Collection = serviceCollection;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public T Resolve<T>() where T : class
    {
        return (T)Resolve(typeof(T));
    }

    public object Resolve(Type type)
    {
        return Provider.GetRequiredService(type);
    }

    public void Memoize<T>() where T : class
    {
        Memoize(typeof(T));
    }

    public void Memoize(Type type)
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

        var resolver = new MemoizedResolver(provider => priorFactory(provider));
        Use(type, provider => resolver.ResolveParameter(provider));
    }

    private void Use<T>(Type builtType, Func<IServiceProvider, T> builder) where T : class
    {
        var currentImplementation = Collection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (currentImplementation != null)
        {
            Collection.Remove(currentImplementation);
        }

        Collection.AddTransient(builtType, builder);
        InvalidateCurrentProvider();
    }
    
    public void Use<T>(Func<IServiceProvider, T> builder) where T : class
    {
        Use(typeof(T), builder);
    }

    public void Use<T>(Func<T> builder) where T : class
    {
        Use(typeof(T), _ => builder());
    }
    
    private void InvalidateCurrentProvider()
    {
        _provider = null;
    }
}