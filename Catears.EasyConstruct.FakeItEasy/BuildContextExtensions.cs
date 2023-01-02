namespace Catears.EasyConstruct.FakeItEasy;

public static class BuildContextExtensions
{
    public static void RegisterFake<T>(this BuildContext context) where T : class
    {
        FakeItEasyMockFactory.RegisterFake<T>(context);
    }

    public static void RegisterFake(this BuildContext context, Type type)
    {
        FakeItEasyMockFactory.RegisterFake(context, type);
    }
}