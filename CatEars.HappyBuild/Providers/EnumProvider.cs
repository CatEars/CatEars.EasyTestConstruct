namespace CatEars.HappyBuild.Providers;

public class EnumProvider
{
    public object RandomEnum<T>()
    {
        return RandomEnum(typeof(T));
    }

    public object RandomEnum(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            var message = $"Expected an enum type to generate random value of enum, but got '{enumType.Name}'";
            throw new ArgumentException(message);
        }

        var values = Enum.GetValues(enumType);
        var random = new Random();
        return values.GetValue(random.Next(values.Length))!;
    }
}