namespace Catears.EasyTestConstruct.Providers;

public record StringProviderOptions(string? VariableName, string? VariableType)
{
    public static StringProviderOptions Default => new(null, null);
}