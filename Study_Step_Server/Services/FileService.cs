using Study_Step_Server.Interfaces;
using Study_Step_Server.Models.DTO;

namespace Study_Step_Server.Services
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory;
        public FileService(string baseDirectory)
        {
            _baseDirectory = baseDirectory;

            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        public byte[]? ConvertFileToByteArray(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath)) { return null; }
            return File.ReadAllBytes(imagePath);
        }


        public string? SaveFile(byte[]? fileBytes, string? fileName)
        {
            try {
                string uniqueFileName = $"{Guid.NewGuid()}_{fileName}"; // Generate unique name_file
                string filePath = Path.Combine(_baseDirectory, uniqueFileName); // Create path

                File.WriteAllBytes(filePath, fileBytes); // Save File

                return filePath;
            }
            catch (Exception ex) { return null; }
        }
    }
}
