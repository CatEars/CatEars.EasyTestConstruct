using System;
using CatEars.HappyBuild;
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
        var animal = (IJungleAnimal)scope.MemoizeAndBuild<Monkey>();
        var food = (IAnimalFood)scope.MemoizeAndBuild<Banana>();
        var repository = scope.MemoizeAndBuild<IAnimalFoodCompatibilityRepository>();
        A.CallTo(() => repository.CanBeFeed(animal, food))
            .Returns(true);
        var feeder = scope.Build<AnimalFeeder>();

        var result = feeder.TryFeedAnimal(animal, food);
        Assert.True(result.WasFed);
        Assert.True(animal.IsFull);
    }
}