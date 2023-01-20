namespace CatEars.HappyBuild;

public interface BuildScope
{
    public BuildContext.Options Options { get; }
    
    public T Resolve<T>() where T : class;

    public void Memoize<T>() where T : class;

    public void Use<T>(Func<IServiceProvider, T> builder) where T : class;

    public void Use<T>(Func<T> builder) where T : class;

    public BuildScope BindParameter<TService, TParam>(TParam value) where TService : class;

    public BuildScope BindNthParameterOfType<TService, TParam>(TParam value, int parameterIndex) where TService : class;

    public BuildScope BindNthParameter<TService, TParam>(TParam value, int index) where TService : class;
}