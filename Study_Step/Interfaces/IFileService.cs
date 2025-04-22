using Study_Step.Models;
using Study_Step.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Study_Step.Interfaces
{
    public interface IFileService
    {
        byte[]? ConvertFileToByteArray(string imagePath);
        BitmapImage? ConvertByteArrayToBitmapImage(byte[]? byteArray);
        byte[]? ConvertBitmapImageToByteArray(BitmapImage? bitmapImage);
        BitmapImage? LoadImage(string? imagePath);
        void SaveFile(FileModelDTO file);
    }
}
