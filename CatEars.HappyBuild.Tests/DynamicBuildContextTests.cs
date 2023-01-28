using Xunit;

namespace CatEars.HappyBuild.Tests;

public class DynamicBuildContextTests
{
    internal record InnerRecord(string Value);

    internal record OuterRecord(InnerRecord Inner);

    internal record DoubleInnerRecord(OuterRecord Inner);

    internal record TripleInnerRecord(DoubleInnerRecord Double);
    
    [Fact]
    public void DynamicContext_WithNoRegistration_CanResolveComplexType()
    {
        var context = new BuildContext(new BuildContext.Options()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        var resolved = context.Scope().Build<OuterRecord>();

        Assert.NotNull(resolved);
        Assert.NotNull(resolved.Inner);
        Assert.NotEmpty(resolved.Inner.Value);
    }

    [Fact]
    public void DynamicContext_WithPreRegisteredType_ResolvesPreRegisteredType()
    {
        var context = new BuildContext(new BuildContext.Options()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        context.Register(() => new InnerRecord("42"));
        var resolved = context.Scope().Build<OuterRecord>();

        Assert.Equal("42", resolved.Inner.Value);
    }

    [Fact]
    public void DynamicContext_WithLongDependencyChain_ResolvesCompletely()
    {
        var context = new BuildContext(new BuildContext.Options()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        
        var result = context.Scope().Build<TripleInnerRecord>();
        
        Assert.NotNull(result);
    }

    [Fact]
    public void Memoize_CalledBeforeMultipleResolves_WillResultInParameterAlwaysBeingMemoized()
    {
        var context = new BuildContext(new BuildContext.Options()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        var scope = context.Scope();
        scope.Memoize<InnerRecord>();

        var a = scope.Build<OuterRecord>();
        var b = scope.Build<OuterRecord>();
        
        Assert.Same(a.Inner, b.Inner);
    }
}