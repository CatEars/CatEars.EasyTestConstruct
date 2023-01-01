namespace Catears.EasyConstruct.FakeItEasy;

public static class FakeItEasyMockFactory
{
    public static void RegisterFake(BuildContext context, Type type)
    {
        context.RegisterFake(type);
    }
}