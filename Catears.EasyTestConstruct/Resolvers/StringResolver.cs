namespace Catears.EasyTestConstruct.Resolvers;

public class StringResolver : IParameterResolver
{
    private string Prefix { get; }
    
    public StringResolver(string prefix)
    {
        Prefix = prefix;
    }
    
    public object ResolveParameter(IServiceProvider _)
    {
        var id = Guid.NewGuid().ToString();
        return $"{Prefix}-{id}";
    }
}