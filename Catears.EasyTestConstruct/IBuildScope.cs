namespace Catears.EasyTestConstruct;

public interface IBuildScope : IDisposable
{
    T Resolve<T>() where T : class;

    object Resolve(Type type);

    void Memoize<T>() where T : class;

    void Memoize(Type type);

    void Use<T>(Func<IServiceProvider, T> builder) where T : class;

    void Use<T>(Func<T> builder) where T : class;

}