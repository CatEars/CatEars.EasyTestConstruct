namespace TooComplexForYouDotCom.OpenGenericInterfaceWithConcreteImplementation;

public class OpenGenericInterfaceImpl<T> : IOpenGenericInterface<T>
{
    public string GetTheType()
    {
        return typeof(T).Name;
    }
}