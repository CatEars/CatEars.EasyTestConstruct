using Catears.EasyConstruct.Providers;
using Catears.EasyConstruct.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catears.EasyConstruct;

public class BuildContext
{

    public record Options(
        RegistrationMode RegistrationMode, 
        Action<BuildContext, Type> MockRegistrationMethod)
    {
        public static Options Default { get; } = new(RegistrationMode.Basic, (_, type) =>
        {
            InternalOptions.Default.MockRegistrationMethod(type);
        });
    }

    internal record InternalOptions(RegistrationMode RegistrationMode, Action<Type> MockRegistrationMethod)
    {
        internal static InternalOptions Default { get; } = new(RegistrationMode.Basic, type =>
        {
            var msg = $"Trying to register abstract type or interface '{type.Name}' without any " +
                      $"defined function that handles such types. Add a `{nameof(Options.MockRegistrationMethod)}` " +
                      "when creating your build context to register mocks for these kinds of types when they " +
                      "are encountered.";
            throw new ArgumentException(msg);
        });
    }

    private ServiceCollection ServiceCollection { get; } = new();

    private InternalOptions BuildOptions { get; }
    
    public BuildContext(Options? options = null)
    {
        options ??= Options.Default;
        BuildOptions = new InternalOptions(
            options.RegistrationMode, 
            type => options.MockRegistrationMethod(this, type));
        ServiceCollection.RegisterBasicValueProviders();
    }

    public void Register(Type type)
    {
        var registrationContext = ServiceRegistrationContext.FromTypeAndBuildOptions(type, BuildOptions);
        ServiceRegistrator.RegisterServiceOrThrow(ServiceCollection, registrationContext);
    }
    
    public void Register<T>() where T : class
    {
        Register(typeof(T));
    }

    public void Register(Type type, Func<IServiceProvider, object> builder)
    {
        ServiceCollection.AddTransient(type, builder);
    }
    
    public void Register<T>(Func<IServiceProvider, T> builder) where T : class
    {
        ServiceCollection.AddTransient(builder);
    }

    public void Register<T>(Func<T> builder) where T : class
    {
        Register(_ => builder());
    }

    public BuildScope Scope()
    {
        return new BuildScope(CopyOf(ServiceCollection));
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}