namespace Catears.EasyConstruct;

public static class QuickConstruct
{

    public static BuildScope AutoScope<T>() where T : class
    {
        var context = new BuildContext(BuildContext.Options.Default with
        {
            RegistrationMode = RegistrationMode.Recursive
        });
        context.Register<T>();
        return context.Scope();
    }

    public static BuildScope AutoScopeWithMockFactory<T>(Action<BuildContext, Type> mockRegistrationMethod) where T : class
    {
        var context = new BuildContext(BuildContext.Options.Default with
        {
            RegistrationMode = RegistrationMode.Recursive,
            MockRegistrationMethod = mockRegistrationMethod
        });
        context.Register<T>();
        return context.Scope();
    }

}