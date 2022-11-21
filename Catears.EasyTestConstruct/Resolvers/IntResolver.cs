using Catears.EasyTestConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Resolvers;

public class IntResolver : IParameterResolver
{
    public object ResolveParameter(IServiceProvider provider)
    {
        var intProvider = provider.GetRequiredService<IntProvider>();
        return intProvider.AnyInt();
    }
}