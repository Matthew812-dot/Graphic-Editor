namespace GraphicEditor;

internal sealed class EllipseTool : ShapeTool
{
    public override void OnMouseDown(Graphics graphics, Pen pen, Point location) => Begin(location);

    public override void OnMouseMove(Graphics graphics, Pen pen, Point location) => Move(location);

    public override void OnMouseUp(Graphics graphics, Pen pen, Point location) => Finish(graphics, pen, location);

    public override void DrawPreview(Graphics graphics, Pen pen)
    {
        Rectangle bounds = Bounds;
        if (bounds.Width > 0 && bounds.Height > 0)
        {
            graphics.DrawEllipse(pen, bounds);
        }
    }
}
