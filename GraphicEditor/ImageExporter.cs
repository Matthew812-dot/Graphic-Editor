using System.Drawing.Imaging;

namespace GraphicEditor;

internal static class ImageExporter
{
    public static void SavePng(Image image, string path)
    {
        string directory = Path.GetDirectoryName(Path.GetFullPath(path))!;
        string temporaryPath = Path.Combine(directory, $"{Guid.NewGuid():N}.tmp");

        var output = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write);
        try
        {
            using (output)
            {
                image.Save(output, ImageFormat.Png);
            }

            File.Move(temporaryPath, path, overwrite: true);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }
}
