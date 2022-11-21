using System.Reflection;
using Catears.EasyTestConstruct.Providers;
using Catears.EasyTestConstruct.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catears.EasyTestConstruct;

public class BuildContext
{
    private ServiceCollection ServiceCollection { get; } =  new();

    public BuildContext()
    {
        ServiceCollection.AddScoped<IntProvider>();
    }
    
    public void Register<T>() where T : class
    {
        var typeDescriptor = typeof(T);
        var constructors = typeDescriptor.GetConstructors();
        if (constructors.Length == 0)
        {
            ServiceCollection.AddScoped<T>();
        }
        else if (constructors.Length == 1)
        {
            RegisterAdvancedBuilder<T>(constructors[0]);
        }
        else
        {
            throw new NotImplementedException("Objects with multiple constructors are not supported");
        }
    }

    public void Register<T>(Func<IServiceProvider, T> builder) where T : class
    {
        ServiceCollection.AddScoped(builder);
    }

    public void Register<T>(Func<T> builder) where T : class
    {
        Register(_ => builder());
    }

    private void RegisterAdvancedBuilder<T>(ConstructorInfo constructor) where T : class
    {
        var parameterDescriptors = constructor.GetParameters();
        var parameterResolvers = Enumerable.Range(0, parameterDescriptors.Length)
            .Select(x => (IParameterResolver) new NullResolver())
            .ToList();
        
        foreach (var parameter in constructor.GetParameters())
        {
            var type = parameter.ParameterType;
            if (type == typeof(int) || type == typeof(Int32))
            {
                parameterResolvers[parameter.Position] = new IntResolver();
            }
            else if (type == typeof(string))
            {
                var prefix = $"{parameter.Name}-{type.Name}";
                parameterResolvers[parameter.Position] = new StringResolver(prefix);
            }
            else if (type.IsEnum)
            {
                parameterResolvers[parameter.Position] = new EnumResolver(type);
            }
            else
            {
                parameterResolvers[parameter.Position] = new RecursiveResolver(type);
            }
        }
        
        ServiceCollection.AddScoped(services =>
        {
            var parameters = parameterResolvers.Select(resolver => resolver.ResolveParameter(services));
            return (T)constructor.Invoke(parameters.ToArray());
        });
    }

    public IBuildScope Scope()
    {
        return new BuildScope(CopyOf(ServiceCollection));
    }

    private static IServiceCollection CopyOf(ServiceCollection serviceCollection)
    {
        return new ServiceCollection { serviceCollection };
    }
}