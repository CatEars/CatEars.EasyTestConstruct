using Catears.EasyConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct.Resolvers;

internal class StringResolver : IParameterResolver
{
    private StringProviderOptions StringProviderOptions { get; }

    public StringResolver(StringProviderOptions stringProviderOptions)
    {
        StringProviderOptions = stringProviderOptions;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        var stringProvider = provider.GetRequiredService<StringProvider>();
        return stringProvider.RandomString(StringProviderOptions);
    }
}