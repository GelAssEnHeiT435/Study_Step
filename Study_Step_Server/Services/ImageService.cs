using Study_Step_Server.Interfaces;

namespace Study_Step_Server.Services
{
    public class ImageService : IImageService
    {
        public byte[]? ConvertImageToByteArray(string? imagePath)
        {
            // Возвращаем null, если путь пустой или файл не существует
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) { return null; }
            return File.ReadAllBytes(imagePath);  // Читаем файл в массив байтов
        }

        // TODO: Add method to save image
    }
}
