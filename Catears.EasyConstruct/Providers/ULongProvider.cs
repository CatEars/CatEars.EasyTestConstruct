namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class ULongProvider
{
    public ulong RandomULong()
    {
        var random = new Random();
        return (ulong) random.NextInt64();
    }
}