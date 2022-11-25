namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class ByteProvider
{
    public byte RandomByte()
    {
        var random = new Random();
        return (byte)random.Next();
    }
}