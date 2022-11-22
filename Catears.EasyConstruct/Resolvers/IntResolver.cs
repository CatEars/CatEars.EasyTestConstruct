namespace Catears.EasyConstruct.Resolvers;

internal class IntResolver : IParameterResolver
{
    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.RandomInt();
    }
}