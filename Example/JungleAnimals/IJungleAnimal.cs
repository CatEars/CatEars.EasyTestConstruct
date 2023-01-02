namespace JungleAnimals;

public interface IJungleAnimal
{

    bool TryEat(IAnimalFood food);

    public bool IsFull { get; }

}