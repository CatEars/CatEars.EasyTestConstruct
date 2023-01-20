namespace CatEars.HappyBuild;

public static class BuildScopeExtensions
{
    public static T MemoizeAndResolve<T>(this BuildScope scope) where T : class
    {
        scope.Memoize<T>();
        return scope.Resolve<T>();
    }

    public static T UseAndResolve<T>(this BuildScope scope, T obj) where T : class
    {
        return scope.UseAndResolve(_ => obj);
    }

    public static T UseAndResolve<T>(this BuildScope scope, Func<T> builder) where T : class
    {
        return scope.UseAndResolve(_ => builder());
    }

    public static T UseAndResolve<T>(this BuildScope scope, Func<IServiceProvider, T> builder) where T : class
    {
        scope.Use(builder);
        return scope.Resolve<T>();
    }
}