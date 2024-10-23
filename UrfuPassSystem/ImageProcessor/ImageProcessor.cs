using System.Drawing;

namespace UrfuPassSystem.ImageProcessor;

public class ImageProcessor : IImageProcessor
{
    public Task<int> CheckImage(string filePath, string destinationPath)
    {
        var bitmap = (Bitmap)Image.FromFile(filePath);
        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
        bitmap.Save(destinationPath);
        return Task.FromResult(0);
    }
}
