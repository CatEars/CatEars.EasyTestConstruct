using Xunit;

namespace Catears.EasyConstruct.Tests;

public class DynamicBuildContextTests
{
    internal record InnerRecord(string Value);

    internal record OuterRecord(InnerRecord Inner);
    
    [Fact]
    public void DynamicContext_WithNoRegistration_CanResolveComplexType()
    {
        var context = new BuildContext(new BuildContext.Options()
        {
            RegistrationMode = RegistrationMode.Dynamic
        });
        var resolved = context.Scope().Resolve<OuterRecord>();
        
        Assert.NotNull(resolved);
        Assert.NotNull(resolved.Inner);
        Assert.NotEmpty(resolved.Inner.Value);
    }
}