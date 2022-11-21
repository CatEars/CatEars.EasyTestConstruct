namespace JungleAnimals.Food;

public class Nectar : IAnimalFood
{

    public Nectar(string flower)
    {
        Flower = flower;
    }
    
    public string Flower { get; private set; }
    
}