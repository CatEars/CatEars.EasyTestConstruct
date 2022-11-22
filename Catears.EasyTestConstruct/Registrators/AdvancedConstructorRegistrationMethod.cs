using System.Reflection;
using Catears.EasyTestConstruct.ResolverFactories;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyTestConstruct.Registrators;

internal static class AdvancedConstructorRegistrationMethod
{

    public static void Register(IServiceCollection serviceCollection, Type serviceType, ConstructorInfo constructor)
    {
        var parameterDescriptors = constructor.GetParameters();
        var sortedByPosition = parameterDescriptors.OrderBy(paramInfo => paramInfo.Position);
        var parameterResolvers = sortedByPosition
            .Select(DefaultParameterResolverFactoryChain.FirstLink.CreateParameterResolverOrThrow)
            .ToList();

        serviceCollection.AddScoped(serviceType, services =>
        {
            var parameters = parameterResolvers.Select(resolver => resolver.ResolveParameter(services));
            return constructor.Invoke(parameters.ToArray());
        });
    }
    
}