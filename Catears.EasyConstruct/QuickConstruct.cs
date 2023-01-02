namespace Catears.EasyConstruct;

public static class QuickConstruct
{
    public static BuildScope AutoScope<T>() where T : class
    {
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive
        });
        context.Register<T>();
        return context.Scope();
    }

    public static BuildScope AutoScopeWithMockFactory<T>(Func<Type, object> mockFactory)
        where T : class
    {
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockFactoryMethod = mockFactory
        });
        context.Register<T>();
        return context.Scope();
    }
}