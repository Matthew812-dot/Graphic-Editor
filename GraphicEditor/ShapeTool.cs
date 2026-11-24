namespace GraphicEditor;

internal abstract class ShapeTool : DrawingTool
{
    protected Point Start { get; private set; }
    protected Point End { get; private set; }

    protected Rectangle Bounds => Rectangle.FromLTRB(
        Math.Min(Start.X, End.X),
        Math.Min(Start.Y, End.Y),
        Math.Max(Start.X, End.X),
        Math.Max(Start.Y, End.Y));

    protected void Begin(Point location)
    {
        Start = location;
        End = location;
    }

    protected void Move(Point location)
    {
        End = location;
    }

    protected void Finish(Graphics graphics, Pen pen, Point location)
    {
        Move(location);
        DrawPreview(graphics, pen);
    }
}
