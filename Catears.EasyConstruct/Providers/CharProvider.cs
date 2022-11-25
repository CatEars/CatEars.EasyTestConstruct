namespace Catears.EasyConstruct.Providers;

public class CharProvider
{
    public char RandomChar()
    {
        var random = new Random();
        return (char)random.Next();
    }
}