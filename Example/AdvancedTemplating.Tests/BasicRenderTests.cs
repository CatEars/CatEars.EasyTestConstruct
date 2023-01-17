using CatEars.HappyBuild;
using Xunit;

namespace AdvancedTemplating.Tests;

public class BasicRenderTests
{

    private BuildContext Context { get; } = new(new BuildContext.Options()
    {
        RegistrationMode = RegistrationMode.Controlled
    });

    public BasicRenderTests()
    {
        Context.Register<P>();
        Context.Register<B>();
        Context.Register<I>();

        Context.Register(typeof(Template<>));
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