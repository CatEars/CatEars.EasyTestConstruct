namespace Catears.EasyConstruct.Providers;

public record StringProviderOptions(string? VariableName, string? VariableType)
{
    public static StringProviderOptions Default => new(null, null);
}