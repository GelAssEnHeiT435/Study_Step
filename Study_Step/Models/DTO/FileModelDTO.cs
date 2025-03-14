namespace Study_Step.Models.DTO
{
    public class FileModelDTO
    {
        public int FileModelId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; } // For example .jpg
        public long Size { get; set; } // size (byte)
        public string MimeType { get; set; }
        public byte[] FileBytes { get; set; }
        public DateTime CreatedAt { get; set; }

        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
