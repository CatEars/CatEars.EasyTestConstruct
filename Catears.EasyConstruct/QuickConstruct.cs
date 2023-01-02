namespace Catears.EasyConstruct;

public static class QuickConstruct
{
    public static BuildScope AutoScope()
    {
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        return context.Scope();
    }

    public static BuildScope AutoScopeWithMockFactory(Func<Type, object> mockFactory)
    {
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Dynamic,
            MockFactoryMethod = mockFactory
        });
        return context.Scope();
    }
}