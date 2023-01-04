using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Resolvers;

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

    public bool Provides(Type type)
    {
        return WantedType == type;
    }
}