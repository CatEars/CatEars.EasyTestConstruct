namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class UShortProvider
{
    public ushort RandomUShort()
    {
        var random = new Random();
        return (ushort)random.Next();
    }
}