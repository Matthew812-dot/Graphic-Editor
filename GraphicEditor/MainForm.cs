using System.Runtime.InteropServices;

namespace GraphicEditor;

public partial class MainForm : Form
{
    private readonly ToolManager tools = new();
    private readonly DrawingCanvas canvas;
    private ToolStripButton? selectedToolButton;
    private Bitmap? colorSwatch;

    public MainForm()
    {
        InitializeComponent();
        canvas = new DrawingCanvas(tools) { Dock = DockStyle.Fill };
        canvasPanel.Controls.Add(canvas);

        var toolbar = new ToolStrip
        {
            Dock = DockStyle.Top,
            GripStyle = ToolStripGripStyle.Hidden,
            Padding = new Padding(0, 0, 0, 6),
            TabStop = true
        };

        AddToolButton(toolbar, "Карандаш", tools.ActiveTool);
        AddToolButton(toolbar, "Линия", new LineTool());
        AddToolButton(toolbar, "Прямоугольник", new RectangleTool());
        AddToolButton(toolbar, "Эллипс", new EllipseTool());
        toolbar.Items.Add(new ToolStripSeparator());

        var colorButton = new ToolStripButton("Цвет…")
        {
            DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
        };
        colorButton.Click += (_, _) => ChooseColor(colorButton);
        UpdateColorButton(colorButton);
        toolbar.Items.Add(colorButton);
        toolbar.Items.Add(new ToolStripLabel("Толщина"));

        var widthSelector = new ToolStripComboBox
        {
            AccessibleName = "Толщина линии",
            AutoSize = false,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 55
        };
        int[] widths = [1, 2, 3, 5, 8, 12, 20];
        foreach (int width in widths)
        {
            widthSelector.Items.Add(width);
        }

        widthSelector.SelectedItem = canvas.StrokeWidth;
        widthSelector.SelectedIndexChanged += (_, _) =>
        {
            if (widthSelector.SelectedItem is int width)
            {
                canvas.StrokeWidth = width;
            }
        };
        toolbar.Items.Add(widthSelector);
        toolbar.Items.Add(new ToolStripSeparator());

        toolbar.Items.Add(new ToolStripButton("Очистить холст", null, (_, _) => canvas.ClearArtwork()));
        toolbar.Items.Add(new ToolStripButton("Сохранить изображение…", null, (_, _) => SaveImage()));
        Controls.Add(toolbar);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            colorSwatch?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void AddToolButton(ToolStrip toolbar, string label, DrawingTool tool)
    {
        var button = new ToolStripButton(label) { Checked = tool == tools.ActiveTool };
        button.Click += (_, _) =>
        {
            if (selectedToolButton is { } previous)
            {
                previous.Checked = false;
            }

            tools.SelectTool(tool);
            selectedToolButton = button;
            button.Checked = true;
        };
        toolbar.Items.Add(button);
        if (button.Checked)
        {
            selectedToolButton = button;
        }
    }

    private void ChooseColor(ToolStripButton button)
    {
        using var dialog = new ColorDialog { Color = canvas.StrokeColor, FullOpen = true };
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            canvas.StrokeColor = dialog.Color;
            UpdateColorButton(button);
        }
    }

    private void UpdateColorButton(ToolStripButton button)
    {
        Color color = canvas.StrokeColor;
        var swatch = new Bitmap(16, 16);
        using (Graphics graphics = Graphics.FromImage(swatch))
        {
            graphics.Clear(color);
            graphics.DrawRectangle(Pens.Gray, 0, 0, 15, 15);
        }

        button.Image = swatch;
        colorSwatch?.Dispose();
        colorSwatch = swatch;
        button.ToolTipText = $"Цвет рисования #{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    private void SaveImage()
    {
        using var dialog = new SaveFileDialog
        {
            Title = "Сохранить изображение",
            Filter = "Изображение PNG (*.png)|*.png",
            DefaultExt = "png",
            AddExtension = true,
            FileName = "Рисунок.png"
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            using Bitmap image = canvas.CopyArtwork();
            ImageExporter.SavePng(image, dialog.FileName);
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or ExternalException)
        {
            MessageBox.Show(this, error.Message, "Не удалось сохранить изображение",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
