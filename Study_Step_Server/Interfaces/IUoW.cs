namespace Study_Step_Server.Interfaces;

public interface IUoW : IDisposable
{ 
    IUserChatRepository UserChats { get; } 
    IMessageRepository Messages { get; }
    IUserRepository Users { get; }
    IChatRepository Chats { get; }
    IFileRepository Files { get; }
    IDeletedMessageRepository DeletedMessages { get; }
    IDeletedChatRepository DeletedChats { get; }
    Task<int> SaveChangesAsync();
}