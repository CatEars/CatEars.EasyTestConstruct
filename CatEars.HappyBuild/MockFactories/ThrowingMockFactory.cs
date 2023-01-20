namespace CatEars.HappyBuild.MockFactories;

internal class ThrowingMockFactory : MockFactory
{
    public object CreateMockFromRawType(Type mockTypeToCreate)
    {
        var message =
            $"You tried to create a mockable type '{mockTypeToCreate.Name}' without configuring a mocking" +
            $" framework for HappyBuild. You should always use HappyBuild together with a mocking framework";
        throw new NotImplementedException(message);
    }

    public T CreateMock<T>() where T : class
    {
        return (T) CreateMockFromRawType(typeof(T));
    }
}