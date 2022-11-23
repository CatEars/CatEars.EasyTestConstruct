namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class FloatProvider
{
    public float RandomFloat()
    {
        var random = new Random();
        return random.NextSingle();
    }
}