using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Resolvers;

public class RecursiveResolver : IParameterResolver
{
    private Type WantedType { get; }
    
    public RecursiveResolver(Type wantedType)
    {
        WantedType = wantedType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.GetRequiredService(WantedType);
    }
}