using Study_Step_Server.Models;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId);
    }
}