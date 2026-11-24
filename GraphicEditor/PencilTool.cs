namespace GraphicEditor;

internal sealed class PencilTool : DrawingTool
{
    private Point lastPoint;

    public override void OnMouseDown(Graphics graphics, Pen pen, Point location)
    {
        lastPoint = location;
        float radius = pen.Width / 2;
        using var brush = new SolidBrush(pen.Color);
        graphics.FillEllipse(brush, location.X - radius, location.Y - radius, pen.Width, pen.Width);
    }

    public override void OnMouseMove(Graphics graphics, Pen pen, Point location)
    {
        if (location == lastPoint)
        {
            return;
        }

        graphics.DrawLine(pen, lastPoint, location);
        lastPoint = location;
    }

    public override void OnMouseUp(Graphics graphics, Pen pen, Point location)
    {
        OnMouseMove(graphics, pen, location);
    }
}
