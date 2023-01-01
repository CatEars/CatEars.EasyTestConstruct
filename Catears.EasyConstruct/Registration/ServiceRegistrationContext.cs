using System.Reflection;

namespace Catears.EasyConstruct.Registration;

internal record ServiceRegistrationContext(
    Type ServiceToRegister, 
    ConstructorInfo[] Constructors,
    bool IsOpenGenericType)
{
    internal static ServiceRegistrationContext FromType(Type service) =>
        new(service, service.GetConstructors(), service.ContainsGenericParameters);
}