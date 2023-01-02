namespace Catears.EasyConstruct.Resolvers;

internal class MemoizedResolver : IParameterResolver
{
    private Func<IServiceProvider, object> Resolver { get; }

    private Func<object>? CapturingClosure { get; set; }

    private Type ProvidedType { get; }
    
    public MemoizedResolver(Func<IServiceProvider, object> resolver, Type providedType)
    {
        Resolver = resolver;
        ProvidedType = providedType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        if (CapturingClosure != null)
        {
            return CapturingClosure();
        }

        return ResolveAndSaveValue(provider);
    }

    public bool Provides(Type type)
    {
        return ProvidedType == type;
    }

    private object ResolveAndSaveValue(IServiceProvider provider)
    {
        var value = Resolver(provider);
        CapturingClosure = () => value;
        return value;
    }
}