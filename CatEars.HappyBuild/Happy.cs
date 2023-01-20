namespace CatEars.HappyBuild;

public static class Happy
{
    public class BuildInstance
    {
        // Intentionally left empty
        internal BuildInstance() {}
    }

    public static BuildInstance Build { get; } = new();
}