using FakeItEasy;

namespace CatEars.HappyBuild.FakeItEasy;

public class FakeItEasyMockFactory : MockFactory
{
    public T CreateMock<T>() where T : class => A.Fake<T>();
}