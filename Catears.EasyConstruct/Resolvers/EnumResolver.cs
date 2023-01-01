using Catears.EasyConstruct.Extensions;

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

        if (Enum.GetValues(enumType).Length == 0)
        {
            throw new ArgumentException(
                $"Cannot create EnumResolver for '{enumType.Name}' as it does not contain any values");
        }

        EnumType = enumType;
    }

    public object ResolveParameter(IServiceProvider provider)
    {
        return provider.RandomEnum(EnumType);
    }
}