using FakeItEasy;
using FakeItEasy.Sdk;

namespace Catears.EasyConstruct.FakeItEasy;

public static class BuildContextExtensions
{
    public static void RegisterFake<T>(this BuildContext context) where T : class
    {
        context.Register(A.Fake<T>);
    }

    public static void RegisterFake(this BuildContext context, Type type)
    {
        context.Register(type, _ => Create.Fake(type));
    }
}