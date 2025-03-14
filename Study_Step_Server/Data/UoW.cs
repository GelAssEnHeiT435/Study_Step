using Study_Step_Server.Interfaces;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Data;

public class UoW : IUoW
{
    private readonly ApplicationContext _context;
    
    private IUserChatRepository _userChat;
    private IMessageRepository _message;
    private IUserRepository _user;
    private IChatRepository _chat;
    private IFileRepository _file;

    public UoW(ApplicationContext context)
    {
        _context = context;
    }

    public IUserChatRepository UserChats => _userChat ??= new UserChatRepository(_context);
    public IMessageRepository Messages => _message ??= new MessageRepository(_context);
    public IUserRepository Users => _user ??= new UserRepository(_context);
    public IChatRepository Chats => _chat ??= new ChatRepository(_context);
    public IFileRepository Files => _file ??= new FileRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose() =>
        _context.Dispose();
}