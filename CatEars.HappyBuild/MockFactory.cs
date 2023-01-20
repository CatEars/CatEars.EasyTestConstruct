namespace CatEars.HappyBuild;

public interface MockFactory
{
    object CreateMock(Type mockTypeToCreate);
}