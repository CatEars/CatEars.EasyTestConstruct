using System.Reflection;

namespace Catears.EasyTestConstruct.Registrators;

internal record ServiceRegistrationContext(Type ServiceToRegister, ConstructorInfo[] Constructors)
{
    public static ServiceRegistrationContext FromType(Type service) => new(service, service.GetConstructors());
}