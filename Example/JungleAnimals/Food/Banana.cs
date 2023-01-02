namespace JungleAnimals.Food;

public class Banana : IAnimalFood
{

    public enum Color
    {
        Green,
        Yellowish,
        Yellow
    }

    public Banana(int length, Color bananaColor)
    {
        Length = length;
        BananaColor = bananaColor;
    }

    public int Length { get; private set; }

    public Color BananaColor { get; private set; }

}