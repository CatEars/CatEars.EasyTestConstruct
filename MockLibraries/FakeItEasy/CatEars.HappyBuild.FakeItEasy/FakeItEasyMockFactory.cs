using FakeItEasy;

namespace CatEars.HappyBuild.FakeItEasy;

public class FakeItEasyMockFactory : MockFactory
{
    public T CreateMock<T>(BuildContext.Options options) where T : class => A.Fake<T>();
}