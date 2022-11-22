namespace Catears.EasyConstruct.Registrators;

internal static class DefaultServiceRegistratorChain
{
    public static AggregateServiceRegistrator FirstLink { get; } = new(new List<IServiceRegistrator>()
    {
        // Most user-defined objects in C# will have a constructor. For most classes and records this is automatically generated.
        // However, for static classes and structs they are not. In those cases you should not be able to register
        // without a builder function so we do not allow automatic service registration for types without a constructor.
        // Only 1+ constructors.
        new SingleConstructorServiceRegistrator(),
        new AttributeSelectedConstructorServiceRegistrator()
    });
}