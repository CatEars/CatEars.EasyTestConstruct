namespace CatEars.HappyBuild.Moq;

public static class HappyBuildExtensions
{
    public static BuildScope AutoScope(this Happy.BuildInstance _)
    {
        var options = new MoqBuildOptions()
        {
            MockFactory = new MoqMockFactory(),
            RegistrationMode = RegistrationMode.Dynamic
        };
        return new BuildContext(options).Scope();
    }
}