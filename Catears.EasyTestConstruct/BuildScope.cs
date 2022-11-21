using Catears.EasyTestConstruct.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct;

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

        Use(provider => priorFactory(provider));
    }

    public void Use<T>(Func<IServiceProvider, T> builder) where T : class
    {
        var currentImplementation = Collection.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
        if (currentImplementation != null)
        {
            Collection.Remove(currentImplementation);
        }
        var resolver = new MemoizedResolver(builder);
        Collection.AddScoped(provider => (T) resolver.ResolveParameter(provider));
    }

    public void Use<T>(Func<T> builder) where T : class
    {
        Use(_ => builder());
    }
}