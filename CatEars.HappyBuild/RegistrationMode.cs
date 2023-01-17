namespace CatEars.HappyBuild;

/// <summary>
/// Configuration for how, and when, classes and interfaces are registered by HappyBuild.
/// </summary>
public enum RegistrationMode
{
    /// <summary>
    /// Dynamic registration mode means that when a class is resolved, any of its dependent interfaces and classes
    /// that are yet to be registered will be registered dynamically as the requested class is resolved.
    ///
    /// This is a great default for HappyBuild as it gives you the most flexibility.
    /// </summary>
    Dynamic = 2,
    
    /// <summary>
    /// Controlled registration mode means that class registration will always be done by instructing HappyBuild what to
    /// register. HappyBuild will not try to register any classes for you.
    ///
    /// This mode is only useful in very specific situations where you want to control exactly which classes HappyBuild
    /// is able to construct.
    /// </summary>
    Controlled = 3
}