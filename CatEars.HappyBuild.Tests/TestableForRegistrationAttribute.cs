using System;

namespace CatEars.HappyBuild.Tests;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
public class TestableForRegistrationAttribute : Attribute
{

}