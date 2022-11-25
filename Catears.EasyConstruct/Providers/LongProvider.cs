namespace Catears.EasyConstruct.Providers;

public class LongProvider
{
    public long RandomLong()
    {
        var random = new Random();
        return random.NextInt64();
    }
}