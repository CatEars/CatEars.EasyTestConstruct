using JungleAnimals.Food;

namespace JungleAnimals.Animals;

public class Piranha : IJungleAnimal
{
    public bool IsFull { get; private set; }

    public bool TryEat(IAnimalFood food)
    {
        if (IsFull || food is not Meat meat) return false;

        Console.WriteLine($"Piranha eats meat that weighs {meat.Weight}");
        IsFull = true;
        return true;
    }
}