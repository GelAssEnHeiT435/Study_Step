using System.Windows.Media.Imaging;

namespace Study_Step.Interfaces
{
    public interface IImageService
    {
        byte[]? ConvertImageToByteArray(string imagePath);
        BitmapImage? ConvertByteArrayToBitmapImage(byte[]? byteArray);
        BitmapImage? LoadImage(string? imagePath);
    }
}
