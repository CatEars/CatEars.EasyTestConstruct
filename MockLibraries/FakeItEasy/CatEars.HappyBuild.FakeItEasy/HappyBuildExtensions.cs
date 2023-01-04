using FakeItEasy.Sdk;

namespace CatEars.HappyBuild.FakeItEasy;

public static class HappyBuildExtensions
{
    public static BuildScope AutoScope(this Happy.BuildInstance _)
    {
        var buildContext = new BuildContext(new BuildContext.Options()
        {
            MockFactoryMethod = Create.Fake,
            RegistrationMode = RegistrationMode.Dynamic
        });
        return buildContext.Scope();
    }
}