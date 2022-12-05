using System;
using Catears.EasyConstruct;
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

    [Theory]
    [InlineData(typeof(Monkey), typeof(Banana))]
    [InlineData(typeof(Piranha), typeof(Meat))]
    [InlineData(typeof(Bird), typeof(Nectar))]
    public void CanBeFed_WithCompatibleAnimals_FeedsAnimals(Type animalType, Type foodType)
    {
        var scope = Fixture.Context.Scope();
        var animal = (IJungleAnimal)scope.MemoizeAndResolve(animalType);
        var food = (IAnimalFood)scope.MemoizeAndResolve(foodType);
        var repository = scope.MemoizeAndResolve<IAnimalFoodCompatibilityRepository>();
        A.CallTo(() => repository.CanBeFeed(animal, food))
            .Returns(true);
        var feeder = scope.Resolve<AnimalFeeder>();

        var result = feeder.TryFeedAnimal(animal, food);
        Assert.True(result.WasFed);
        Assert.True(animal.IsFull);
    }
}