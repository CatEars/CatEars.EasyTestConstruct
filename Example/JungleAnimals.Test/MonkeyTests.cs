using CatEars.HappyBuild;
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
        var scope = Fixture.Context.Scope();
        var banana = scope.Build<Banana>();
        var monkey = scope.Build<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.True(result);
    }

    [Fact]
    public void TryFeed_WithMeat_ReturnsFalse()
    {
        var scope = Fixture.Context.Scope();
        var banana = scope.Build<Meat>();
        var monkey = scope.Build<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.False(result);
    }

    [Fact]
    public void TryFeed_WithGreenBanana_ReturnsTrue()
    {
        var scope = Fixture.Context.Scope();
        var banana = scope.UseAndBuild(provider => new Banana(provider.RandomInt(), Banana.Color.Green));
        var monkey = scope.Build<Monkey>();

        var result = monkey.TryEat(banana);

        Assert.True(result);
        Assert.Equal(Banana.Color.Green, banana.BananaColor);
    }
}