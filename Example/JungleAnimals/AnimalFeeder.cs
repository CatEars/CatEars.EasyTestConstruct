using JungleAnimals.Animals;

namespace JungleAnimals;

public class AnimalFeeder
{
    private IAnimalFoodCompatibilityRepository foodCompatibilityRepository;

    public AnimalFeeder(IAnimalFoodCompatibilityRepository foodCompatibilityRepository)
    {
        this.foodCompatibilityRepository = foodCompatibilityRepository;
    }

    public (bool WasFed, string FeederExpression) TryFeedAnimal(IJungleAnimal animal, IAnimalFood food)
    {
        if (!foodCompatibilityRepository.CanBeFeed(animal, food))
            return IncompatibleFoodAndAnimal(animal, food);

        var wasFed = animal.TryEat(food);
        return wasFed ? WasFed() : WasNotFed();
    }

    private static (bool, string) WasNotFed()
    {
        return (false, "Nah, they were all full");
    }

    private static (bool, string) WasFed()
    {
        return (true, "They sure are hungry");
    }

    private static (bool, string) IncompatibleFoodAndAnimal(IJungleAnimal animal, IAnimalFood food)
    {
        return (false, $"{animal.GetType().Name} dont eat {food.GetType().Name}!");
    }
}