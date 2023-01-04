namespace CatEars.HappyBuild;

public static class Happy
{
    public class BuildInstance
    {
        // Intentionally left empty
        
        /*
         * When building EasyConstruct I wanted the API to make sense and be rather minimal.
         * This means that the simplest way of using the API needs to have exactly one entry-point
         * and it should basically only have one function you need to call.
         *
         * To provide this I decided that extension methods were the way to go. Since we always need
         * to inject the mock creation factory of an external library like FakeItEasy or Moq it would
         * be rather easy in such a library to just define the "AutoScope" method. Even if multiple
         * authors would implement the same method, users of EasyConstruct should tend to have
         * exactly one mocking library to worry about, thus they would only be able to call
         * exactly one function which would be written for their specific mocking library.
         */

        internal BuildInstance() {}
    }

    public static BuildInstance Build { get; } = new();
}