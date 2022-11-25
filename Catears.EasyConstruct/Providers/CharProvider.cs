namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class CharProvider
{
    public char RandomChar()
    {
        var random = new Random();
        return (char)random.Next();
    }
}