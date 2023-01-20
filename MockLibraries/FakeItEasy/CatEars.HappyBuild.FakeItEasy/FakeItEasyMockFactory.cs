using FakeItEasy.Sdk;

namespace CatEars.HappyBuild.FakeItEasy;

public class FakeItEasyMockFactory : MockFactory
{
    public object CreateMock(Type mockTypeToCreate) => Create.Fake(mockTypeToCreate);
}