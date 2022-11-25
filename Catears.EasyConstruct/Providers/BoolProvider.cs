namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class BoolProvider
{
    public bool RandomBool()
    {
        var random = new Random();
        return random.Next(0, 2) == 1;
    }
}