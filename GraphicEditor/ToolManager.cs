namespace GraphicEditor;

internal sealed class ToolManager
{
    public DrawingTool ActiveTool { get; private set; } = new PencilTool();

    public void SelectTool(DrawingTool tool)
    {
        ActiveTool = tool;
    }
}
