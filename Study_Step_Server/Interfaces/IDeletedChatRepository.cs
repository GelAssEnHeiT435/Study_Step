using Study_Step_Server.Models;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IDeletedChatRepository : IRepository<DeletedChat>
    {
        Task DeleteChatForUserAsync(int userId, int chatId);
        Task RestoreChatAsync(int senderId, int chatId);
    }
}
