namespace Catears.EasyConstruct.Resolvers;

internal class FuncResolver : IParameterResolver
{
    private Func<IServiceProvider, object> Generator { get; }
    
    public FuncResolver(Func<IServiceProvider, object> generator)
    {
        Generator = generator;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return Generator(provider);
    }
}