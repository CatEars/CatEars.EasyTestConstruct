using System.Reflection;

namespace Catears.EasyConstruct;

internal record ServiceRegistrationContext(Type ServiceToRegister, ConstructorInfo[] Constructors)
{
    public static ServiceRegistrationContext FromType(Type service) => new(service, service.GetConstructors());
}