using Moq;

namespace CatEars.HappyBuild.Moq;

public static class BuildScopeExtensions
{
    public static Mock<T> MockFor<T>(this BuildScope scope) where T : class
    {
        var options = scope.Options;
        if (options is not MoqBuildOptions moqOptions)
        {
            throw new ArgumentException($"Expected options of scope to be of type {nameof(MoqBuildOptions)}");
        }

        if (!moqOptions.StoredMocks.TryGetValue(typeof(T), out var obj))
        {
            throw new ArgumentException($"Expected to find Mock<{typeof(T).Name}> but found `null`");
        }
        
        if (obj is not Mock<T> mock)
        {
            throw new ArgumentException($"Expected to find Mock<{typeof(T).Name}> but found {obj.GetType().Name}");
        }

        return mock;
    }
}