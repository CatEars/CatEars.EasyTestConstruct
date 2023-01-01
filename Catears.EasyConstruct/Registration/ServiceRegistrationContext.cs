using System.Reflection;

namespace Catears.EasyConstruct.Registration;

internal record ServiceRegistrationContext(
    Type ServiceToRegister, 
    ConstructorInfo[] Constructors,
    bool IsOpenGenericType,
    bool IsPrimitiveType,
    bool IsMockIntendedType,
    BuildContext.InternalOptions RegistrationOptions)
{
    internal static ServiceRegistrationContext FromType(Type service) =>
        new(service, 
            service.GetConstructors(), 
            service.ContainsGenericParameters, 
            service.IsPrimitive || typeof(string) == service,
            service.IsInterface || service.IsAbstract,
            BuildContext.InternalOptions.Default);

    internal static ServiceRegistrationContext FromTypeAndBuildOptions(Type service, 
        BuildContext.InternalOptions options) =>
        new(service,
            service.GetConstructors(),
            service.ContainsGenericParameters,
            service.IsPrimitive || typeof(string) == service,
            service.IsInterface || service.IsAbstract,
            options);
    
    
}