using CatEars.HappyBuild;
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
        var scope = buildContext.Scope();
        scope.Memoize<Monkey>();

        var monkey = scope.Build<Monkey>();

        Assert.IsType<Monkey>(monkey);
    }

}