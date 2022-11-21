using JungleAnimals.Food;

namespace JungleAnimals.Animals;

public class Monkey : IJungleAnimal
{
    public bool IsFull { get; private set; }

    public bool TryEat(IAnimalFood food)
    {
        if (IsFull || food is not Banana banana) return false;

        Console.WriteLine($"Monkey eats {banana.BananaColor} banana of length {banana.Length} ");
        IsFull = true;
        return true;
    }
}