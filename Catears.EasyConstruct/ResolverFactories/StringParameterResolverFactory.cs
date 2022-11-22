using System.Reflection;
using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Resolvers;

namespace Catears.EasyConstruct.ResolverFactories;

internal class StringParameterResolverFactory : IParameterResolverFactory
{
    public bool TryCreateParameterResolver(ParameterInfo parameterInfo, out IParameterResolver? resolver)
    {
        var type = parameterInfo.ParameterType;
        if (type == typeof(string))
        {
            var options = StringProviderOptions.Default with
            {
                VariableName = parameterInfo.Name,
                VariableType = type.Name
            };
            resolver = new StringResolver(options);
            return true;
        }

        resolver = null;
        return false;
    }
}