using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Resolvers;

public class MemoizedResolver : IParameterResolver
{
    private Func<IServiceProvider, object> Resolver { get; }

    private Func<object>? Closure { get; set; }

    public MemoizedResolver(Func<IServiceProvider, object> resolver)
    {
        Resolver = resolver;
    }
    
    public object ResolveParameter(IServiceProvider provider)
    {
        if (Closure != null) return Closure!();
        var value = Resolver(provider);
        Closure = () => value;
        return value;
    }
}