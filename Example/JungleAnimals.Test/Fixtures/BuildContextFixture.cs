using CatEars.HappyBuild;
using CatEars.HappyBuild.FakeItEasy;
using JungleAnimals.Animals;
using JungleAnimals.Food;

namespace JungleAnimals.Test.Fixtures;

public class BuildContextFixture
{
    public BuildContext Context { get; } = new(new BuildContext.Options()
    {
        RegistrationMode = RegistrationMode.Controlled
    });

    public BuildContextFixture()
    {
        Context.Register<Bird>();
        Context.Register<Monkey>();
        Context.Register<Piranha>();

        Context.Register<Banana>();
        Context.Register<Meat>();
        Context.Register<Nectar>();

        Context.Register<AnimalFeeder>();
        Context.Register<BigOlAnimalFoodDatabase>();

        Context.RegisterFake<IAnimalFood>();
        Context.RegisterFake<IAnimalFoodCompatibilityRepository>();
        Context.RegisterFake<IJungleAnimal>();
    }

}