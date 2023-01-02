using FakeItEasy;
using FakeItEasy.Sdk;

namespace Catears.EasyConstruct.FakeItEasy;

public static class QuickFakeItEasyConstruct
{
    public static BuildScope AutoScope()
    {
        return QuickConstruct.AutoScopeWithMockFactory(Create.Fake);
    }
}