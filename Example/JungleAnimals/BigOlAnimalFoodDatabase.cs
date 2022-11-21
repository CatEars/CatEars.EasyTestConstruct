using JungleAnimals.Animals;
using JungleAnimals.Food;

namespace JungleAnimals;

public class BigOlAnimalFoodDatabase : IAnimalFoodCompatibilityRepository
{
    public bool CanBeFeed(IJungleAnimal animal, IAnimalFood food)
    {
        return (food is Banana && animal is Monkey) ||
               (food is Nectar && animal is Bird) ||
               (food is Meat && animal is Piranha);
    }
}