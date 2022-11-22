using System.Linq;
using Catears.EasyConstruct;
using JungleAnimals.Animals;
using JungleAnimals.Food;
using JungleAnimals.Test.Fixtures;
using Xunit;

namespace JungleAnimals.Test;

public class MonkeyTests : IClassFixture<BuildContextFixture>
{
    private BuildContextFixture Fixture { get; }

    public MonkeyTests(BuildContextFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void TryFeed_WithBanana_ReturnsTrue()
    {
        using var scope = Fixture.Context.Scope();
        var banana = scope.Resolve<Banana>();
        var monkey = scope.Resolve<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.True(result);
    }

    [Fact]
    public void TryFeed_WithMeat_ReturnsFalse()
    {
        using var scope = Fixture.Context.Scope();
        var banana = scope.Resolve<Meat>();
        var monkey = scope.Resolve<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.False(result);
    }

    [Fact]
    public void TryFeed_WithGreenBanana_ReturnsTrue()
    {
        using var scope = Fixture.Context.Scope();
        var banana = scope.UseAndResolve(provider => new Banana(provider.RandomInt(), Banana.Color.Green));
        var monkey = scope.Resolve<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.True(result);
        Assert.Equal(Banana.Color.Green, banana.BananaColor);
    }
}