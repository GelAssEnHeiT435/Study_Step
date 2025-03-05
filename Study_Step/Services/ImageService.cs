using Study_Step.Interfaces;
using System.IO;
using System.Windows.Media.Imaging;

namespace Study_Step.Services
{
    public class ImageService : IImageService
    {
        public byte[]? ConvertImageToByteArray(string? imagePath)
        {
            // Возвращаем null, если путь пустой или файл не существует
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) { return null; }
            return File.ReadAllBytes(imagePath);  // Читаем файл в массив байтов
        }

        public BitmapImage? ConvertByteArrayToBitmapImage(byte[]? byteArray)
        {
            if (byteArray is null) return null;

            BitmapImage bitmapImage = new BitmapImage();
            using (var stream = new MemoryStream(byteArray))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        public BitmapImage? LoadImage(string? imagePath)
        {
            if (imagePath is null) { return null; }

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);  // Путь к файлу
            bitmap.EndInit();

            return bitmap;
        }
    }
}
