namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class SByteProvider
{
    public sbyte RandomSByte()
    {
        var random = new Random();
        return (sbyte)random.Next();
    } 
}