using System.Reflection;

namespace CatEars.HappyBuild.Registration;

internal record ServiceRegistrationContext(
    Type ServiceToRegister,
    ConstructorInfo[] Constructors,
    bool IsOpenGenericType,
    bool IsBasicType,
    bool IsMockIntendedType)
{
    internal static ServiceRegistrationContext FromType(Type service) =>
        new(service,
            service.GetConstructors(),
            service.ContainsGenericParameters,
            service.IsPrimitive || typeof(string) == service,
            service.IsInterface || (service.IsAbstract && !service.IsSealed));

}