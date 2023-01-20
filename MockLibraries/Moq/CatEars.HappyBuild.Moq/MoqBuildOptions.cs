namespace CatEars.HappyBuild.Moq;

public class MoqBuildOptions : BuildContext.Options
{
    internal Dictionary<Type, object> StoredMocks { get; init; } = new();

    public override BuildContext.Options Copy()
    {
        return new MoqBuildOptions()
        {
            MockFactory = MockFactory,
            StoredMocks = new Dictionary<Type, object>(StoredMocks),
            RegistrationMode = RegistrationMode
        };
    }
}