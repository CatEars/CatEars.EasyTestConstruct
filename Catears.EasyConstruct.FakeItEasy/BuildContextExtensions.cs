using FakeItEasy;

namespace Catears.EasyConstruct.FakeItEasy;

public static class BuildContextExtensions
{
    public static void RegisterFake<T>(this BuildContext context) where T : class
    {
        context.Register(A.Fake<T>);
    }
}