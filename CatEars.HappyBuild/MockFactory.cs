namespace CatEars.HappyBuild;

public interface MockFactory
{
    T CreateMock<T>(BuildContext.Options options) where T : class;
}