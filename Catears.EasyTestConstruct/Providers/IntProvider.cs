namespace Catears.EasyTestConstruct.Providers;

public class IntProvider
{
    public int AnyInt()
    {
        var random = new Random();
        return random.Next(0, 10000);
    }
}