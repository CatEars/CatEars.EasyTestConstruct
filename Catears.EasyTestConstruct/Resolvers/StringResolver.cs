using Catears.EasyTestConstruct.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Resolvers;

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