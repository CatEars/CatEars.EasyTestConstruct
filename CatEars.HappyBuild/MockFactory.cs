namespace CatEars.HappyBuild;

public interface MockFactory
{
    T CreateMock<T>() where T : class;
}