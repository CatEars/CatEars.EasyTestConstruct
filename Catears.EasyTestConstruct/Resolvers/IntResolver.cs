namespace Catears.EasyTestConstruct.Resolvers;

public class IntResolver : IParameterResolver
{
    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.RandomInt();
    }
}