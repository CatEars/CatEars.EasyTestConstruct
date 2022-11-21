using JungleAnimals.Food;

namespace JungleAnimals.Animals;

public class Bird : IJungleAnimal
{
    public bool IsFull { get; private set; }

    public bool TryEat(IAnimalFood food)
    {
        if (IsFull || food is not Nectar nectar) return false;

        Console.WriteLine($"Bird eats nectar from flower {nectar.Flower}");
        IsFull = true;
        return true;

    }

}