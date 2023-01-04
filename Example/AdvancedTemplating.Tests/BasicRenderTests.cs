using CatEars.HappyBuild;
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
        var mySpecifiedGenericType = typeof(Template<P>);
        var myOpenGenericType = typeof(Template<>);

        Context.Register(typeof(Template<>));
        //Context.Register(typeof(Template<>), typeof(Template<>));
        /*Context.Register<Template<P>>();
        Context.Register<Template<B>>();
        Context.Register<Template<I>>();*/
    }

    [Fact]
    public void CanRenderAParagraphTag()
    {
        var scope = Context.Scope();
        var sut = scope.Resolve<Template<P>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }

    [Fact]
    public void CanRenderATextWithEmphasisTag()
    {
        var scope = Context.Scope();
        var sut = scope.Resolve<Template<B>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }

    [Fact]
    public void CanRenderAnItalizisedTextTag()
    {
        var scope = Context.Scope();
        var sut = scope.Resolve<Template<I>>();

        var value = sut.Render();

        Assert.IsType<string>(value);
        Assert.NotEmpty(value);
    }
}