namespace Catears.EasyTestConstruct.Providers;

public class IntProvider
{
    public int RandomInt(int low = 0, int high = 10000)
    {
        var random = new Random();
        return random.Next(low, high);
    }
}