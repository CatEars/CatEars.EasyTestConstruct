using CatEars.HappyBuild.Extensions;
using CatEars.HappyBuild.FakeItEasy;
using FakeItEasy;
using Xunit;

namespace CatEars.HappyBuild.Tests;

public class EasyConstructTests
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
        var scope = Easy.Build.AutoScope();

        var resolved = scope.Resolve<RecordWithSingleConstructorContainingComplexParameter>();

        Assert.NotNull(resolved);
        Assert.NotNull(resolved._);
    }

    [Fact]
    public void AutoScope_WithInterfaceInHierarchy_CanMockInterface()
    {
        var scope = Easy.Build.AutoScope();
        var mock = scope.MemoizeAndResolve<TestInterface>();
        A.CallTo(() => mock.GetValue()).Returns("42");
        
        var resolved = scope.Resolve<TestInterfaceWrapper>();
        var result = resolved.GetWrappedValue();

        Assert.Equal("[42]", result);
    }
}