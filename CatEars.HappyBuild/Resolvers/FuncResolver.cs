namespace CatEars.HappyBuild.Resolvers;

internal class FuncResolver : IParameterResolver
{
    private Func<IServiceProvider, object> Generator { get; }

    private Type ProvidedType { get; }
    
    public FuncResolver(Func<IServiceProvider, object> generator, Type providedType)
    {
        Generator = generator;
        ProvidedType = providedType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return Generator(provider);
    }

    public bool Provides(Type type)
    {
        return ProvidedType == type;
    }
}