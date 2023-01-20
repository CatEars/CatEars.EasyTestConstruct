using Moq;

namespace CatEars.HappyBuild.Moq;

public class MoqMockFactory : MockFactory
{
    public T CreateMock<T>(BuildContext.Options options) where T : class
    {
        if (options is not MoqBuildOptions moqOptions)
        {
            throw new ArgumentException("Expected MoqBuildOptions", nameof(options));
        }

        if (moqOptions.StoredMocks.TryGetValue(typeof(T), out var priorMock))
        {
            return (T)priorMock;
        }
        
        var mock = new Mock<T>();
        moqOptions.StoredMocks.Add(typeof(T), mock);
        return mock.Object;
    }
}