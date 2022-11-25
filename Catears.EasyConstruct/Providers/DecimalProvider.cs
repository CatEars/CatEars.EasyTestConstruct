namespace Catears.EasyConstruct.Providers;

public class DecimalProvider
{
    public decimal RandomDecimal()
    {
        var random = new Random();
        return new decimal(random.NextDouble());
    }
}