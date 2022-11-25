namespace Catears.EasyConstruct.Providers;

public class UShortProvider
{
    public ushort RandomUShort()
    {
        var random = new Random();
        return (ushort)random.Next();
    }
}