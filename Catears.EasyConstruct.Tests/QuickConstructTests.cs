using Catears.EasyConstruct.Extensions;
using Catears.EasyConstruct.FakeItEasy;
using Catears.EasyConstruct.Tests.Registrators;
using FakeItEasy;
using Xunit;

namespace Catears.EasyConstruct.Tests;

public class QuickConstructTests
{

    public interface TestInterface
    {
        string GetValue();
    }

    public record TestInterfaceWrapper(TestInterface TestInterface)
    {
        public string GetWrappedValue() => $"[{TestInterface.GetValue()}]";
    }

    [Fact]
    public void AutoScope_WithComplexType_BuildsScopeThatCanResolveType()
    {
        var scope = QuickConstruct.AutoScope<RecordWithSingleConstructorContainingComplexParameter>();

        var resolved = scope.Resolve<RecordWithSingleConstructorContainingComplexParameter>();

        Assert.NotNull(resolved);
        Assert.NotNull(resolved._);
    }

    [Fact]
    public void AutoScopeWithFakeItEasy_WithInterfaceInHierarchy_CanMockInterface()
    {
        var scope = QuickFakeItEasyConstruct.AutoScope<TestInterfaceWrapper>();
        var mock = scope.MemoizeAndResolve<TestInterface>();
        A.CallTo(() => mock.GetValue()).Returns("42");

        var resolved = scope.Resolve<TestInterfaceWrapper>();
        var result = resolved.GetWrappedValue();

        Assert.Equal("[42]", result);
    }
}