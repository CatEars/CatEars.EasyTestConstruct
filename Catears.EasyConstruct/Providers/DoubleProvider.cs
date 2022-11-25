namespace Catears.EasyConstruct.Providers;

public class DoubleProvider
{
    public double RandomDouble()
    {
        var random = new Random();
        return random.NextDouble();
    }
}