namespace AdvancedTemplating;

public record P(string Text) : IRenderable
{
    public string RenderSelf()
    {
        return Text;
    }
}

public record I(string Text) : IRenderable
{
    public string RenderSelf()
    {
        return $"Image the following has a slight slant: {Text}";
    }
}

public record B(string Text) : IRenderable
{
    public string RenderSelf()
    {
        return $">> {Text} <<";
    }
}