using Catears.EasyConstruct;
using TooComplexForYouDotCom.OpenGenericInterfaceWithConcreteImplementation;
using Xunit;

namespace TooComplexForYouDotCom.Tests;

public class OpenGenericTests
{
    public record RecWithDependency(IOpenGenericInterface<string> Dependency);

    [Fact]
    public void CannotDeductInterfaceFromOnlyGenericImplementation()
    {
        var context = new BuildContext();
        context.Register(typeof(OpenGenericInterfaceImpl<>));
        context.Register<RecWithDependency>();
        context.Scope().Resolve<RecWithDependency>();
    }

    [Fact]
    public void CannotAutomaticallyResolveDependencyWithSpecificImplementation()
    {
        var context = new BuildContext();
        context.Register<OpenGenericInterfaceImpl<string>>();
        context.Register<RecWithDependency>();
        context.Scope().Resolve<RecWithDependency>();
    }

}