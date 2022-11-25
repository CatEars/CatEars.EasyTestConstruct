using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Catears.EasyConstruct;

internal static class ServiceRegistrator
{
    public static void RegisterServiceOrThrow(IServiceCollection collection, ServiceRegistrationContext context)
    {
        // Most user-defined objects in C# will have a constructor. For most classes and records this is automatically generated.
        // However, for static classes and structs they are not. In those cases you should not be able to register
        // without a builder function so we do not allow automatic service registration for types without a constructor.
        // Only 1+ constructors.
        if (context.Constructors.Length == 1)
        {
            Register(collection, context.ServiceToRegister, context.Constructors.First());
            return;
        }

        bool IsPreferredConstructor(ConstructorInfo info) =>
            Attribute.IsDefined(info, typeof(PreferredConstructorAttribute));
        
        var markedConstructor = context.Constructors.FirstOrDefault(IsPreferredConstructor);
        if (markedConstructor != null)
        {
            Register(collection, context.ServiceToRegister, markedConstructor);
            return ;
        }

        var msg = $"Constructor for type {context.ServiceToRegister.Name} did not contain exactly 1 constructor, " +
                  $"and it did not contain a constructor marked as {nameof(PreferredConstructorAttribute)} and can " +
                  $"therefore not be registered for automatically constructing";
        throw new ArgumentException(msg);
    }

    private static void Register(IServiceCollection serviceCollection, Type serviceType, ConstructorInfo constructor)
    {
        var parameterDescriptors = constructor.GetParameters();
        var sortedByPosition = parameterDescriptors.OrderBy(paramInfo => paramInfo.Position);
        var parameterResolvers = sortedByPosition
            .Select(ParameterResolverFactory.GetResolverForType)
            .ToList();

        serviceCollection.AddTransient(serviceType, services =>
        {
            var parameters = parameterResolvers.Select(resolver => resolver.ResolveParameter(services));
            return constructor.Invoke(parameters.ToArray());
        });
    }
}