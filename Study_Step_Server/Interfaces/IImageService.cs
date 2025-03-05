namespace Study_Step_Server.Interfaces
{
    public interface IImageService
    {
        byte[]? ConvertImageToByteArray(string imagePath);
    }
}
