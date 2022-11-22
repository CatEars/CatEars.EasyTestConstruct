namespace Catears.EasyConstruct.Providers;

[BasicValueProvider]
public class StringProvider
{
    public string RandomString(StringProviderOptions? options = null)
    {
        options ??= StringProviderOptions.Default;
        var strings = new List<string>();
        AddIfNotNullOrEmpty(strings, options.VariableName);
        AddIfNotNullOrEmpty(strings, options.VariableType);
        AddIfNotNullOrEmpty(strings, GenerateId());
        return string.Join("-", strings);
    }

    private static string GenerateId()
    {
        return Nanoid.Nanoid.Generate();
    }

    private void AddIfNotNullOrEmpty(List<string> strings, string? candidate)
    {
        if (string.IsNullOrEmpty(candidate)) return;
        strings.Add(candidate);
    }
}