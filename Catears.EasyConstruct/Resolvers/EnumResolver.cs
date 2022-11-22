namespace Catears.EasyConstruct.Resolvers;

internal class EnumResolver : IParameterResolver
{
    private Type EnumType { get; }

    public EnumResolver(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            var message = $"Cannot create an EnumResolver for type '{enumType.Name}' as it is not an Enum";
            throw new ArgumentException(message);
        }

        EnumType = enumType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.RandomEnum(EnumType);
    }
}