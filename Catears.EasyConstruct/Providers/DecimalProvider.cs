namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class DecimalProvider
{
    public decimal RandomDecimal()
    {
        var random = new Random();
        return new decimal(random.NextDouble());
    }
}