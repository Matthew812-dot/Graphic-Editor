namespace GraphicEditor;

internal sealed class LineTool : ShapeTool
{
    public override void OnMouseDown(Graphics graphics, Pen pen, Point location) => Begin(location);

    public override void OnMouseMove(Graphics graphics, Pen pen, Point location) => Move(location);

    public override void OnMouseUp(Graphics graphics, Pen pen, Point location) => Finish(graphics, pen, location);

    public override void DrawPreview(Graphics graphics, Pen pen)
    {
        if (Start != End)
        {
            graphics.DrawLine(pen, Start, End);
        }
    }
}
