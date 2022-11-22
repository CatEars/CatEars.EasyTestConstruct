
namespace Catears.EasyTestConstruct.Resolvers;

public class MemoizedResolver : IParameterResolver
{
    private Func<IServiceProvider, object> Resolver { get; }

    private Func<object>? CapturingClosure { get; set; }

    public MemoizedResolver(Func<IServiceProvider, object> resolver)
    {
        Resolver = resolver;
    }
    
    public object ResolveParameter(IServiceProvider provider)
    {
        if (CapturingClosure != null)
        {
            return CapturingClosure();
        }
        return ResolveAndSaveValue(provider);
    }

    private object ResolveAndSaveValue(IServiceProvider provider)
    {
        var value = Resolver(provider);
        CapturingClosure = () => value;
        return value;
    }
}