namespace CatEars.HappyBuild.Extensions;

public static class BuildScopeExtensions
{
    public static T MemoizeAndResolve<T>(this BuildScope scope) where T : class
    {
        return (T)scope.MemoizeAndResolve(typeof(T));
    }

    public static object MemoizeAndResolve(this BuildScope scope, Type type)
    {
        scope.Memoize(type);
        return scope.Resolve(type);
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