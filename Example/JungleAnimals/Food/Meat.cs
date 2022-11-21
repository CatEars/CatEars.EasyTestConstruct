namespace JungleAnimals.Food;

public class Meat : IAnimalFood
{

    public Meat(int weight)
    {
        Weight = weight;
    }
    
    public int Weight { get; private set; }
    
}