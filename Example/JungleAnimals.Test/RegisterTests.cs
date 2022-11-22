using Catears.EasyTestConstruct;
using JungleAnimals.Animals;
using Xunit;

namespace JungleAnimals.Test;

public class RegisterTests
{

    [Fact]
    public void Register_WithMemoizedMonkey_CanThenResolveMonkey()
    {
        var buildContext = new BuildContext();
        buildContext.Register<Monkey>();
        using var scope = buildContext.Scope();
        scope.Memoize<Monkey>();

        var bird = scope.Resolve<Monkey>();

        Assert.IsType<Monkey>(bird);
    }
    
}