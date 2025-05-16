using Study_Step_Server.Models;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IDeletedMessageRepository : IRepository<DeletedMessage>
    {
        Task<bool> IsMessageDeletedForOtherUser(int userId, int messageId);
        Task DeleteChatsMessagesAsync(int userId, int chatId);
    }
}
