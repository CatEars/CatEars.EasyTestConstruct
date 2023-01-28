namespace CatEars.HappyBuild;

public static class BuildScopeExtensions
{
    public static T MemoizeAndBuild<T>(this BuildScope scope) where T : class
    {
        scope.Memoize<T>();
        return scope.Build<T>();
    }

    public static T UseAndBuild<T>(this BuildScope scope, T obj) where T : class
    {
        return scope.UseAndBuild(_ => obj);
    }

    public static T UseAndBuild<T>(this BuildScope scope, Func<T> builder) where T : class
    {
        return scope.UseAndBuild(_ => builder());
    }

    public static T UseAndBuild<T>(this BuildScope scope, Func<IServiceProvider, T> builder) where T : class
    {
        scope.Use(builder);
        return scope.Build<T>();
    }
}