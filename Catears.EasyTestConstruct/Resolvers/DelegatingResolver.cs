using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Resolvers;

internal class DelegatingResolver : IParameterResolver
{
    private Type WantedType { get; }

    public DelegatingResolver(Type wantedType)
    {
        WantedType = wantedType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.GetRequiredService(WantedType);
    }
}