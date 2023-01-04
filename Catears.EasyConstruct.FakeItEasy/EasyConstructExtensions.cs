using FakeItEasy.Sdk;

namespace Catears.EasyConstruct.FakeItEasy;

public static class EasyConstructExtensions
{

    public static BuildScope AutoScope(this Easy.ConstructionInstance _)
    {
        var buildContext = new BuildContext(new BuildContext.Options()
        {
            MockFactoryMethod = Create.Fake,
            RegistrationMode = RegistrationMode.Dynamic
        });
        return buildContext.Scope();
    }
    
}