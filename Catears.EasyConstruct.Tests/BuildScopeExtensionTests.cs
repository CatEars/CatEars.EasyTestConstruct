using Xunit;

namespace Catears.EasyConstruct.Tests;

public class BuildScopeExtensionTests
{
    private record SampleRecord(string StringValue);
    
    [Fact]
    public void MemoizeAndResolve_WhenCalledMultipleTimes_ReturnsSameObject()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        using var scope = buildContext.Scope();
        
        var firstSampleRecord = scope.MemoizeAndResolve<SampleRecord>();
        var secondSampleRecord = scope.Resolve<SampleRecord>();
        
        Assert.Same(firstSampleRecord, secondSampleRecord);
    }
    
    [Fact]
    public void MemoizeAndResolve_WhenCalledAfterResolve_ReturnsANewObject()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        using var scope = buildContext.Scope();
        
        var firstSampleRecord = scope.Resolve<SampleRecord>();
        var secondSampleRecord = scope.MemoizeAndResolve<SampleRecord>();

        Assert.NotSame(firstSampleRecord, secondSampleRecord);
    }
    
    [Fact]
    public void MemoizeAndResolve_ReturnsValidObject()
    {
        using var scope = CreateSampleBuildScope();

        var sampleRecord = scope.MemoizeAndResolve<SampleRecord>();
        
        Assert.NotNull(sampleRecord);
        Assert.IsType<SampleRecord>(sampleRecord);
        Assert.NotEmpty(sampleRecord.StringValue);
    }

    private record FirstRecord();

    private record SecondRecord(FirstRecord FirstRecord);

    private record ThirdRecord(SecondRecord SecondRecord);

    private record FourthRecord(ThirdRecord ThirdRecord);

    [Fact]
    public void MemoizeAndResolve_WhenCalledWithObjectHierarchy_OnlyMemoizesTheSpecificallyUsedType()
    {
        var buildContext = new BuildContext();
        buildContext.Register<FirstRecord>();
        buildContext.Register<SecondRecord>();
        buildContext.Register<ThirdRecord>();
        buildContext.Register<FourthRecord>();
        using var scope = buildContext.Scope();
        
        var fourthRecord = scope.MemoizeAndResolve<FourthRecord>();
        var newThirdRecord = scope.Resolve<ThirdRecord>();
        
        Assert.NotSame(newThirdRecord, fourthRecord.ThirdRecord);
        Assert.NotSame(newThirdRecord.SecondRecord, fourthRecord.ThirdRecord.SecondRecord);
        Assert.NotSame(newThirdRecord.SecondRecord.FirstRecord, fourthRecord.ThirdRecord.SecondRecord.FirstRecord);
    }
    
    private static IBuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}