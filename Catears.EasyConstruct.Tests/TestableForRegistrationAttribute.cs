using System;

namespace Catears.EasyConstruct.Tests;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Struct)]
public class TestableForRegistrationAttribute : Attribute
{

}