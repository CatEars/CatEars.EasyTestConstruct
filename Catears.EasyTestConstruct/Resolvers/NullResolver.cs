namespace Catears.EasyTestConstruct.Resolvers;

public class NullResolver : IParameterResolver
{
    public object ResolveParameter(IServiceProvider provider)
    {
        throw new InvalidOperationException();
    }
}