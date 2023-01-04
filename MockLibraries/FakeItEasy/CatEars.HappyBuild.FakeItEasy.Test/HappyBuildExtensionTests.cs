using CatEars.HappyBuild.Extensions;
using FakeItEasy;
using Xunit;

namespace CatEars.HappyBuild.FakeItEasy.Test;

public class HappyBuildExtensionTests
{

    public interface TestInterface
    {
        string GetValue();
    }

    public record TestInterfaceWrapper(TestInterface TestInterface)
    {
        public string GetWrappedValue() => $"[{TestInterface.GetValue()}]";
    }

    private record EmptyRecord;
    
    private record RecordContainingOtherRecord(EmptyRecord Inner);
    
    [Fact]
    public void AutoScope_WithComplexType_BuildsScopeThatCanResolveType()
    {
        var scope = Happy.Build.AutoScope();

        var resolved = scope.Resolve<RecordContainingOtherRecord>();

        Assert.NotNull(resolved);
        Assert.NotNull(resolved.Inner);
    }

    [Fact]
    public void AutoScope_WithInterfaceInHierarchy_CanMockInterface()
    {
        var scope = Happy.Build.AutoScope();
        var mock = scope.MemoizeAndResolve<TestInterface>();
        A.CallTo(() => mock.GetValue()).Returns("42");
        
        var resolved = scope.Resolve<TestInterfaceWrapper>();
        var result = resolved.GetWrappedValue();

        Assert.Equal("[42]", result);
    }
}