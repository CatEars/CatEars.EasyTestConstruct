namespace Catears.EasyTestConstruct.Registrators;

internal static class DefaultServiceRegistratorChain
{
    public static AggregateServiceRegistrator FirstLink = new(new List<IServiceRegistrator>()
    {
        new NoConstructorServiceRegistrator(),
        new SingleConstructorServiceRegistrator(),
        new AttributeSelectedConstructorServiceRegistrator()
    });
}