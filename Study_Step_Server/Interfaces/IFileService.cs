namespace Study_Step_Server.Interfaces
{
    public interface IFileService
    {
        byte[]? ConvertFileToByteArray(string imagePath);
        string? SaveFile(byte[]? fileBytes, string? fileName);
    }
}
