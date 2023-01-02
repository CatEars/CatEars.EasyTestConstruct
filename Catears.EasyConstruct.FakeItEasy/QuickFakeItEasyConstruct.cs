using FakeItEasy;
using FakeItEasy.Sdk;

namespace Catears.EasyConstruct.FakeItEasy;

public static class QuickFakeItEasyConstruct
{
    public static BuildScope AutoScope<T>() where T : class
    {
        return QuickConstruct.AutoScopeWithMockFactory<T>(Create.Fake);
    }
}