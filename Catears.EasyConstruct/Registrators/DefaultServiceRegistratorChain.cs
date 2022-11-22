namespace Catears.EasyConstruct.Registrators;

internal static class DefaultServiceRegistratorChain
{
    public static AggregateServiceRegistrator FirstLink { get; } = new(new List<IServiceRegistrator>()
    {
        new NoConstructorServiceRegistrator(),
        new SingleConstructorServiceRegistrator(),
        new AttributeSelectedConstructorServiceRegistrator()
    });
}