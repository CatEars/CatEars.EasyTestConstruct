namespace Catears.EasyConstruct.Resolvers;

public class FloatResolver : IParameterResolver
{
    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.RandomFloat();
    }
}