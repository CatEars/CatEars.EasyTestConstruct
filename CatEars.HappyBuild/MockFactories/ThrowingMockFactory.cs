namespace CatEars.HappyBuild.MockFactories;

internal class ThrowingMockFactory : MockFactory
{
    public T CreateMock<T>(BuildContext.Options _) where T : class
    {
        var message =
            $"You tried to create a mockable type '{typeof(T).Name}' without configuring a mocking" +
            $" framework for HappyBuild. You should always use HappyBuild together with a mocking framework";
        throw new NotImplementedException(message);
    }
}