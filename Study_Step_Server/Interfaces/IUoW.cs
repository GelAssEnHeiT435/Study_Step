namespace Study_Step_Server.Interfaces;

public interface IUoW : IDisposable
{ 
    IUserChatRepository UserChats { get; } 
    IMessageRepository Messages { get; }
    IUserRepository Users { get; }
    IChatRepository Chats { get; }
    IFileRepository Files { get; }
    Task<int> SaveChangesAsync();

}