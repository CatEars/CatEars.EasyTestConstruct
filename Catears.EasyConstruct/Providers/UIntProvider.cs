namespace Catears.EasyConstruct.Providers;

public class UIntProvider
{
    public uint RandomUInt()
    {
        var random = new Random();
        return (uint)random.Next();
    }
}