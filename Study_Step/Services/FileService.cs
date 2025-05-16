using Newtonsoft.Json;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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
        public byte[]? ConvertBitmapImageToByteArray(BitmapImage? bitmapImage)
        {
            if (bitmapImage == null) return null;

            BitmapEncoder encoder = new PngBitmapEncoder(); // или другой формат
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
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

        public async Task SendFileAsync(FileModel file)
        {
            var buffer = new byte[81920];
            file.CancellationTokenSource = new CancellationTokenSource();

            using HttpClient client = new HttpClient();
            await using var fileStream = File.OpenRead(file.Path);
            var content = new ProgressableStreamContent(fileStream, 81920, file.CancellationTokenSource.Token)
            {
                Progress = (sentBytes, totalBytes) =>
                {
                    file.Progress = (int)((double)sentBytes / totalBytes * 100);
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post,
                                                 $"http://localhost:5000/api/fileupload/upload")
            {
                Content = content
            };
            request.Headers.Add("X-FileName", WebUtility.UrlEncode(file.Name));
            request.Headers.Add("X-FileSize", file.Size.ToString());

            var response = await client.SendAsync(request,
                                                  HttpCompletionOption.ResponseHeadersRead,
                                                  file.CancellationTokenSource.Token);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(responseJson, new { Path = "", Size = 0L });
            file.Path = result.Path;
            file.Status = SendingStatus.Success;
        }
    }
}
