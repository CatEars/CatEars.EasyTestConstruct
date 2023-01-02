using FakeItEasy;
using FakeItEasy.Sdk;

namespace Catears.EasyConstruct.FakeItEasy;

public static class FakeItEasyMockFactory
{
    public static void RegisterFake<T>(BuildContext context) where T : class
    {
        context.Register(A.Fake<T>);
    }

    public static void RegisterFake(BuildContext context, Type type)
    {
        context.Register(type, _ => Create.Fake(type));
    }
}