namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class SByteProvider
{
    public sbyte GetSByte()
    {
        var random = new Random();
        return (sbyte)random.Next();
    } 
}