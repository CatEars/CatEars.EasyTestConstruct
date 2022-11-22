namespace Catears.EasyConstruct;

public static class BuildScopeExtensions
{
    public static T MemoizeAndResolve<T>(this IBuildScope scope) where T : class
    {
        return (T)scope.MemoizeAndResolve(typeof(T));
    }

    public static object MemoizeAndResolve(this IBuildScope scope, Type type)
    {
        scope.Memoize(type);
        return scope.Resolve(type);
    }

    public static T UseAndResolve<T>(this IBuildScope scope, T obj) where T : class
    {
        return scope.UseAndResolve(_ => obj);
    }

    public static T UseAndResolve<T>(this IBuildScope scope, Func<T> builder) where T : class
    {
        return scope.UseAndResolve(_ => builder());
    }

    public static T UseAndResolve<T>(this IBuildScope scope, Func<IServiceProvider, T> builder) where T : class
    {
        scope.Use(builder);
        return scope.Resolve<T>();
    }
}