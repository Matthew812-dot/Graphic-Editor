namespace GraphicEditor;

partial class MainForm
{
    private Panel canvasPanel = null!;

    private void InitializeComponent()
    {
        canvasPanel = new Panel();
        SuspendLayout();
        //
        // canvasPanel
        //
        canvasPanel.BackColor = Color.White;
        canvasPanel.BorderStyle = BorderStyle.FixedSingle;
        canvasPanel.Dock = DockStyle.Fill;
        canvasPanel.Name = "canvasPanel";
        //
        // MainForm
        //
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        ClientSize = new Size(960, 640);
        Controls.Add(canvasPanel);
        MinimumSize = new Size(480, 320);
        Name = "MainForm";
        Padding = new Padding(16);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Графический редактор";
        ResumeLayout(false);
    }
}
