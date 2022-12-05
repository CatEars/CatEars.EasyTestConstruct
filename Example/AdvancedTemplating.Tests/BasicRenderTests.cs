using Catears.EasyConstruct;
using Xunit;

namespace AdvancedTemplating.Tests;

public class BasicRenderTests
{

    private BuildContext Context { get; } = new();

    public BasicRenderTests()
    {
        Context.Register<P>();
        Context.Register<B>();
        Context.Register<I>();
        Context.Register<Template<P>>();
        Context.Register<Template<B>>();
        Context.Register<Template<I>>();
    }
    
    [Fact]
    public void CanRenderAParagraphTag()
    {
        using var scope = Context.Scope();
        var sut = scope.Resolve<Template<P>>();

        var value = sut.Render();
        
        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }
    
    [Fact]
    public void CanRenderATextWithEmphasisTag()
    {
        using var scope = Context.Scope();
        var sut = scope.Resolve<Template<B>>();

        var value = sut.Render();
        
        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }
    
    [Fact]
    public void CanRenderAnItalizisedTextTag()
    {
        using var scope = Context.Scope();
        var sut = scope.Resolve<Template<I>>();

        var value = sut.Render();
        
        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }
}