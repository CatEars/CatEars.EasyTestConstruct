namespace AdvancedTemplating;

public class Template<T> where T : IRenderable
{

    private T TemplatedType { get; }

    public Template(T templatedType)
    {
        this.TemplatedType = templatedType;
    }

    public string Render()
    {
        var type = TemplatedType.GetType();
        var fullName = type.FullName!;
        return $"<{fullName}>{TemplatedType.RenderSelf()}</{fullName}>";
    }

}