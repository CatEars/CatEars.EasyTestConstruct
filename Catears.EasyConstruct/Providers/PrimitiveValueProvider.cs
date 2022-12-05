namespace Catears.EasyConstruct.Providers;

public class PrimitiveValueProvider
{
    private Random Random { get; } = new();

    public bool RandomBool()
    {
        return Random.Next(0, 2) == 1;
    }

    public byte RandomByte()
    {
        return (byte)Random.Next();
    }

    public char RandomChar()
    {
        return (char)Random.Next();
    }

    public decimal RandomDecimal()
    {
        return new(Random.NextDouble());
    }

    public double RandomDouble()
    {
        return Random.NextDouble();
    }

    public float RandomFloat()
    {
        return Random.NextSingle();
    }

    public int RandomInt(int low = 0, int high = 10000)
    {
        return Random.Next(low, high);
    }

    public long RandomLong()
    {
        return Random.NextInt64();
    }

    public sbyte RandomSByte()
    {
        return (sbyte)Random.Next();
    }

    public short RandomShort()
    {
        return (short)Random.Next();
    }

    public uint RandomUInt()
    {
        return (uint)Random.Next();
    }

    public ulong RandomULong()
    {
        return (ulong)Random.NextInt64();
    }

    public ushort RandomUShort()
    {
        return (ushort)Random.Next();
    }
}