using Study_Step_Server.Models;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int userId, int chatId);
        Task<Message?> GetLastUndeletedMessageAsync(int chatId, int userId);
        Task UpdateMessageWithFiles(Message updatedMessage);
        Task<int> GetUnreadMessagesCount(int chatId, int userId);
        Task MarkAllAsRead(int chatId, int userId);
        Task<bool> isReadMessageByIdAsync(int messageId);
    }
}