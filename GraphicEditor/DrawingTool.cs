namespace GraphicEditor;

internal abstract class DrawingTool
{
    public abstract void OnMouseDown(Graphics graphics, Pen pen, Point location);
    public abstract void OnMouseMove(Graphics graphics, Pen pen, Point location);
    public abstract void OnMouseUp(Graphics graphics, Pen pen, Point location);

    public virtual void DrawPreview(Graphics graphics, Pen pen)
    {
    }
}
