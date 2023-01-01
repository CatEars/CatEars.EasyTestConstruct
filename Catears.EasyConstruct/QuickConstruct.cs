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

    public static BuildScope AutoScopeWithMockFactory<T>(Action<BuildContext, Type> mockRegistrationMethod) where T : class
    {
        var context = new BuildContext(new()
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockRegistrationMethod = mockRegistrationMethod
        });
        context.Register<T>();
        return context.Scope();
    }

}