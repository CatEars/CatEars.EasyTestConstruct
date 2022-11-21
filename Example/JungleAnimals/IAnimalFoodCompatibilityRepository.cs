namespace JungleAnimals;

public interface IAnimalFoodCompatibilityRepository
{

    bool CanBeFeed(IJungleAnimal animal, IAnimalFood food);

}