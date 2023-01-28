using Xunit;

namespace CatEars.HappyBuild.Tests;

public class BuildScopeExtensionTests
{
    private record SampleRecord(string StringValue);

    private static readonly BuildContext.Options ControlledRegistrationOptions = new()
    {
        RegistrationMode = RegistrationMode.Controlled
    };
    
    [Fact]
    public void MemoizeAndResolve_WhenCalledMultipleTimes_ReturnsSameObject()
    {
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<SampleRecord>();
        var scope = buildContext.Scope();

        var firstSampleRecord = scope.MemoizeAndBuild<SampleRecord>();
        var secondSampleRecord = scope.Build<SampleRecord>();

        Assert.Same(firstSampleRecord, secondSampleRecord);
    }

    [Fact]
    public void MemoizeAndResolve_WhenCalledAfterResolve_ReturnsANewObject()
    {
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<SampleRecord>();
        var scope = buildContext.Scope();

        var firstSampleRecord = scope.Build<SampleRecord>();
        var secondSampleRecord = scope.MemoizeAndBuild<SampleRecord>();

        Assert.NotSame(firstSampleRecord, secondSampleRecord);
    }

    [Fact]
    public void MemoizeAndResolve_ReturnsValidObject()
    {
        var scope = CreateSampleBuildScope();

        var sampleRecord = scope.MemoizeAndBuild<SampleRecord>();

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
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<FirstRecord>();
        buildContext.Register<SecondRecord>();
        buildContext.Register<ThirdRecord>();
        buildContext.Register<FourthRecord>();
        var scope = buildContext.Scope();

        var fourthRecord = scope.MemoizeAndBuild<FourthRecord>();
        var newThirdRecord = scope.Build<ThirdRecord>();

        Assert.NotSame(newThirdRecord, fourthRecord.ThirdRecord);
        Assert.NotSame(newThirdRecord.SecondRecord, fourthRecord.ThirdRecord.SecondRecord);
        Assert.NotSame(newThirdRecord.SecondRecord.FirstRecord, fourthRecord.ThirdRecord.SecondRecord.FirstRecord);
    }

    [Fact]
    public void UseAndResolve_WhenCalled_WillResolveAValidObject()
    {
        var scope = CreateSampleBuildScope();
        var isCalled = false;

        var result = scope.UseAndBuild(provider =>
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
        var scope = CreateSampleBuildScope();
        var count = 0;

        scope.UseAndBuild(provider =>
        {
            ++count;
            return new SampleRecord(provider.RandomString());
        });
        scope.Build<SampleRecord>();
        scope.Build<SampleRecord>();

        Assert.Equal(3, count);
    }

    [Fact]
    public void UseAndResolve_CalledAfterResolve_WillCallProvidedBuilderFunction()
    {
        var scope = CreateSampleBuildScope();
        var count = 0;

        scope.Build<SampleRecord>();
        scope.UseAndBuild(provider =>
        {
            ++count;
            return new SampleRecord(provider.RandomString());
        });

        Assert.Equal(1, count);
    }

    [Fact]
    public void UseAndResolve_CalledTwice_WillUseProvidedBuilderFunctionRespectively()
    {
        var scope = CreateSampleBuildScope();
        var firstCount = 0;
        var secondCount = 0;

        scope.Build<SampleRecord>();
        scope.UseAndBuild(provider =>
        {
            ++firstCount;
            return new SampleRecord(provider.RandomString());
        });
        scope.UseAndBuild(provider =>
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
        var scope = CreateSampleBuildScope();

        var resolvedObj = scope.UseAndBuild(obj);

        Assert.Same(obj, resolvedObj);
    }

    [Fact]
    public void UseAndResolve_CalledMultipleTimesWithObject_WillReturnSameObject()
    {
        var obj = new object();
        var scope = CreateSampleBuildScope();

        var resolvedObj1 = scope.UseAndBuild(obj);
        var resolvedObj2 = scope.Build<object>();

        Assert.Same(obj, resolvedObj1);
        Assert.Same(obj, resolvedObj2);
    }

    [Fact]
    public void UseAndResolve_CanBeCalledWithBasicBuilder()
    {
        var scope = CreateSampleBuildScope();

        var result = scope.UseAndBuild(() => new SampleRecord("123"));

        Assert.NotNull(result);
        Assert.IsType<SampleRecord>(result);
        Assert.Equal("123", result.StringValue);
    }

    private static BuildScope CreateSampleBuildScope()
    {
        var buildContext = new BuildContext(ControlledRegistrationOptions);
        buildContext.Register<SampleRecord>();
        return buildContext.Scope();
    }
}