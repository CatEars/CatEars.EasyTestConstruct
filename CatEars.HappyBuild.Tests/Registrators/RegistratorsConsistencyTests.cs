using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Xunit;

namespace CatEars.HappyBuild.Tests.Registrators;

public class RegistratorsConsistencyTests
{

    [Fact]
    public void AllTestsThatTestsAllAvailableClassesForRegistration_IncludesAllRelevantClasses()
    {
        var expectedClassesToTest = GetType()
            .Assembly
            .GetTypes()
            .Where(IsClassTestableForRegistration)
            .ToHashSet();

        var result = GetType()
            .Assembly
            .GetTypes()
            .Where(type => type.IsClass && type.GetMethods().Any(TestsForAllClasses))
            .SelectMany(type => type.GetMethods().Where(TestsForAllClasses));

        foreach (var method in result)
        {
            var assumedTestedClasses = method.CustomAttributes
                .Where(x => x.AttributeType == typeof(InlineDataAttribute) && x.ConstructorArguments.Count > 0)
                .Select(x => x.ConstructorArguments[0])
                .Where(x => x.Value is ReadOnlyCollection<CustomAttributeTypedArgument>)
                .Select(x => x.Value as ReadOnlyCollection<CustomAttributeTypedArgument>)
                .Where(x => x!.Any(o => IsClassTestableForRegistration((Type?)o.Value)))
                .Select(x => (Type)x!.First(o => IsClassTestableForRegistration((Type)o.Value!)).Value!)
                .ToHashSet();
            foreach (var classToTestFor in expectedClassesToTest)
            {
                var message =
                    $"Class {classToTestFor.Name} was not tested for by {method.DeclaringType!.Name}::{method.Name}";
                Assert.True(assumedTestedClasses.Contains(classToTestFor), message);
            }
        }
    }

    private static bool IsClassTestableForRegistration(Type? type)
    {
        if (type == null) return false;
        return Attribute.IsDefined(type, typeof(TestableForRegistrationAttribute));
    }


    private static bool TestsForAllClasses(MethodInfo method)
    {
        return Attribute.IsDefined(method, typeof(TestsAllAvailableClassesForRegistrationAttribute));
    }
}