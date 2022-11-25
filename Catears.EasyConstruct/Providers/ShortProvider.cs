namespace Catears.EasyConstruct.Providers;

public class ShortProvider
{
    public short RandomShort()
    {
        var random = new Random();
        return (short)random.Next();
    }
}