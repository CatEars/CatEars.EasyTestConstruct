namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class DoubleProvider
{
    public double RandomDouble()
    {
        var random = new Random();
        return random.NextDouble();
    }
}