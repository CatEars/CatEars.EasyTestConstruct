using System.Reflection;

namespace Catears.EasyConstruct;

internal record ServiceRegistrationContext(
    Type ServiceToRegister, 
    ConstructorInfo[] Constructors,
    bool IsOpenGenericType)
{
    public static ServiceRegistrationContext FromType(Type service) =>
        new(service, service.GetConstructors(), service.ContainsGenericParameters);
}