using Newtonsoft.Json;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Study_Step.Services
{
    public class FileService : IFileService
    {

        public byte[]? ConvertFileToByteArray(string? imagePath)
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

        public void SaveFile(FileModelDTO file)
        {
            //try
            //{
            //    string uniqueFileName = $"{file.Name}"; 
            //    string filePath = Path.Combine(_baseDirectory, uniqueFileName); // Create path

            //    File.WriteAllBytes(filePath, file.FileBytes); // Save File
            //}
            //catch (Exception ex) { }
        }
    }
}
