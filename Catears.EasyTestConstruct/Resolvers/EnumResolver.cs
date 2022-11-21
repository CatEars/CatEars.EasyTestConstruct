namespace Catears.EasyTestConstruct.Resolvers;

public class EnumResolver : IParameterResolver
{
    private Type EnumType { get; }

    public EnumResolver(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException();
        }

        EnumType = enumType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        var values = Enum.GetValues(EnumType);
        var random = new Random();
        return values.GetValue(random.Next(values.Length))!;
    }
}