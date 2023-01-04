using System.Reflection;
using CatEars.HappyBuild.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CatEars.HappyBuild.Resolvers;

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

    public bool Provides(Type type)
    {
        return type == typeof(string);
    }

    public static StringResolver CreateFromParamInfo(ParameterInfo info)
    {
        return new StringResolver(StringProviderOptions.Default with
        {
            VariableName = info.Name,
            VariableType = info.ParameterType.Name
        });
    }
}