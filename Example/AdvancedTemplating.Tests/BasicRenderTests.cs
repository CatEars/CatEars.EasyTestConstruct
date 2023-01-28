using CatEars.HappyBuild;
using Xunit;

namespace AdvancedTemplating.Tests;

public class BasicRenderTests
{
    private BuildContext Context { get; } = new(new BuildContext.Options());
    
    [Fact]
    public void CanRenderAParagraphTag()
    {
        var scope = Context.Scope();
        var sut = scope.Build<Template<P>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }

    [Fact]
    public void CanRenderATextWithEmphasisTag()
    {
        var scope = Context.Scope();
        var sut = scope.Build<Template<B>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }

    [Fact]
    public void CanRenderAnItalizisedTextTag()
    {
        var scope = Context.Scope();
        var sut = scope.Build<Template<I>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }
}