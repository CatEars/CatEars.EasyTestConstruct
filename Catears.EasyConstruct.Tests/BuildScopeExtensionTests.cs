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

    [Fact]
    public void UseAndResolve_WhenCalled_WillResolveAValidObject()
    {
        using var scope = CreateSampleBuildScope();
        var isCalled = false;

        var result = scope.UseAndResolve(provider =>
        {
            isCalled = true;
            return new SampleRecord(provider.RandomString());
        });

        Assert.True(isCalled);
        Assert.NotNull(result);
        Assert.IsType<SampleRecord>(result);
        Assert.NotEmpty(result.StringValue);
    }
    
    [Fact]
    public void UseAndResolve_WhenCalledMultipleTimes_WillUseProvidedBuildFunction()
    {
        using var scope = CreateSampleBuildScope();
        var count = 0;
        
        scope.UseAndResolve(provider =>
        {
            ++count;
            return new SampleRecord(provider.RandomString());
        });
        scope.Resolve<SampleRecord>();
        scope.Resolve<SampleRecord>();

        Assert.Equal(3, count);
    }

    [Fact]
    public void UseAndResolve_CalledAfterResolve_WillCallProvidedBuilderFunction()
    {
        using var scope = CreateSampleBuildScope();
        var count = 0;

        scope.Resolve<SampleRecord>();
        scope.UseAndResolve(provider =>
        {
            ++count;
            return new SampleRecord(provider.RandomString());
        });
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void UseAndResolve_CalledTwice_WillUseProvidedBuilderFunctionRespectively()
    {
        using var scope = CreateSampleBuildScope();
        var firstCount = 0;
        var secondCount = 0;

        scope.Resolve<SampleRecord>();
        scope.UseAndResolve(provider =>
        {
            ++firstCount;
            return new SampleRecord(provider.RandomString());
        });
        scope.UseAndResolve(provider =>
        {
            ++secondCount;
            return new SampleRecord(provider.RandomString());
        });
        
        Assert.Equal(1, firstCount);
        Assert.Equal(1, secondCount);
    }

    [Fact]
    public void UseAndResolve_CalledWithObject_WillReturnSameObject()
    {
        var obj = new object();
        using var scope = CreateSampleBuildScope();
        
        var resolvedObj = scope.UseAndResolve(obj);
        
        Assert.Same(obj, resolvedObj);
    }
    
    [Fact]
    public void UseAndResolve_CalledMultipleTimesWithObject_WillReturnSameObject()
    {
        var obj = new object();
        using var scope = CreateSampleBuildScope();
        
        var resolvedObj1 = scope.UseAndResolve(obj);
        var resolvedObj2 = scope.Resolve<object>();
        
        Assert.Same(obj, resolvedObj1);
        Assert.Same(obj, resolvedObj2);
    }

    [Fact]
    public void UseAndResolve_CanBeCalledWithBasicBuilder()
    {
        using var scope = CreateSampleBuildScope();

        var result = scope.UseAndResolve(() => new SampleRecord("123"));
        
        Assert.NotNull(result);
        Assert.IsType<SampleRecord>(result);
        Assert.Equal("123", result.StringValue);
    }
    
    private static BuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext();
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}