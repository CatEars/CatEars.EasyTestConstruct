using System;
using CatEars.HappyBuild.Extensions;
using FakeItEasy;
using JungleAnimals.Animals;
using JungleAnimals.Food;
using JungleAnimals.Test.Fixtures;
using Xunit;

namespace JungleAnimals.Test;

public class AnimalFeederTests : IClassFixture<BuildContextFixture>
{
    private BuildContextFixture Fixture { get; }


    public AnimalFeederTests(BuildContextFixture fixture)
    {
        Fixture = fixture;
    }

    [Fact]
    public void CanBeFed_WithCompatibleAnimals_FeedsAnimals()
    {
        var scope = Fixture.Context.Scope();
        var animal = (IJungleAnimal)scope.MemoizeAndResolve<Monkey>();
        var food = (IAnimalFood)scope.MemoizeAndResolve<Banana>();
        var repository = scope.MemoizeAndResolve<IAnimalFoodCompatibilityRepository>();
        A.CallTo(() => repository.CanBeFeed(animal, food))
            .Returns(true);
        var feeder = scope.Resolve<AnimalFeeder>();

        var result = feeder.TryFeedAnimal(animal, food);
        Assert.True(result.WasFed);
        Assert.True(animal.IsFull);
    }
}