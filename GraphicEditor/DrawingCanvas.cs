using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace GraphicEditor;

internal sealed class DrawingCanvas : Control
{
    private readonly ToolManager tools;
    private Bitmap? artwork;
    private DrawingTool? gestureTool;
    private Pen? gesturePen;

    public DrawingCanvas(ToolManager tools)
    {
        this.tools = tools;
        BackColor = Color.White;
        Cursor = Cursors.Cross;
        SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color StrokeColor { get; set; } = Color.Black;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int StrokeWidth { get; set; } = 3;

    public void ClearArtwork()
    {
        EndGesture();
        EnsureArtwork();
        using Graphics graphics = Graphics.FromImage(artwork!);
        graphics.Clear(Color.White);
        Invalidate();
    }

    public Bitmap CopyArtwork()
    {
        EnsureArtwork();
        return (Bitmap)artwork!.Clone();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        EnsureArtwork();
        e.Graphics.DrawImageUnscaled(artwork!, Point.Empty);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        if (gestureTool is { } tool && gesturePen is { } pen)
        {
            tool.DrawPreview(e.Graphics, pen);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left || !ClientRectangle.Contains(e.Location) || gestureTool is not null)
        {
            return;
        }

        var tool = tools.ActiveTool;
        var pen = new Pen(StrokeColor, StrokeWidth)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round
        };

        gestureTool = tool;
        gesturePen = pen;
        Capture = true;
        using Graphics graphics = CreateDrawingGraphics();
        tool.OnMouseDown(graphics, pen, e.Location);
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (gestureTool is { } tool && gesturePen is { } pen)
        {
            using Graphics graphics = CreateDrawingGraphics();
            tool.OnMouseMove(graphics, pen, e.Location);
            Invalidate();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Left && gestureTool is { } tool && gesturePen is { } pen)
        {
            using (Graphics graphics = CreateDrawingGraphics())
            {
                tool.OnMouseUp(graphics, pen, e.Location);
            }

            EndGesture();
        }
    }

    protected override void OnMouseCaptureChanged(EventArgs e)
    {
        base.OnMouseCaptureChanged(e);
        if (!Capture && gestureTool is not null)
        {
            EndGesture();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            gestureTool = null;
            gesturePen?.Dispose();
            gesturePen = null;
            artwork?.Dispose();
        }

        base.Dispose(disposing);
    }

    private Graphics CreateDrawingGraphics()
    {
        EnsureArtwork();
        Graphics graphics = Graphics.FromImage(artwork!);
        graphics.SetClip(ClientRectangle);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        return graphics;
    }

    private void EnsureArtwork()
    {
        int width = Math.Max(ClientSize.Width, artwork?.Width ?? 1);
        int height = Math.Max(ClientSize.Height, artwork?.Height ?? 1);
        if (artwork?.Size == new Size(width, height))
        {
            return;
        }

        // Keep the full drawing when the window temporarily gets smaller.
        var expanded = new Bitmap(width, height);
        using (Graphics graphics = Graphics.FromImage(expanded))
        {
            graphics.Clear(Color.White);
            if (artwork is { } previous)
            {
                graphics.DrawImageUnscaled(previous, Point.Empty);
                previous.Dispose();
            }
        }

        artwork = expanded;
    }

    private void EndGesture()
    {
        gestureTool = null;
        gesturePen?.Dispose();
        gesturePen = null;
        Capture = false;
        Invalidate();
    }
}
