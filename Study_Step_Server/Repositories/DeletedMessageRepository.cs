using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;

namespace Study_Step_Server.Repositories
{
    public class DeletedMessageRepository : Repository<DeletedMessage>, IDeletedMessageRepository
    {
        public DeletedMessageRepository(ApplicationContext context) : base(context) { }

        public async Task<bool> IsMessageDeletedForOtherUser(int userId, int messageId)
        {
            return await _dbSet.AnyAsync(dm => dm.MessageId == messageId
                                             && dm.UserId == userId);
        }

        public async Task DeleteChatsMessagesAsync(int userId, int chatId)
        {
            DateTime time = DateTime.UtcNow;

            var userMessages = await _context.Messages.Where(m => m.ChatId == chatId)
                                                      .Where(m => !_context.DeletedMessages
                                                                           .Any(dm => dm.MessageId == m.MessageId && dm.UserId == userId))
                                                      .ToListAsync();

            var deletedMessages = userMessages.Select(m => new DeletedMessage
            {
                MessageId = m.MessageId,
                UserId = userId,
                ChatId = chatId,
                DeletedAt = time,
                IsDeletedForEveryone = false,
            });

            await _context.DeletedMessages.AddRangeAsync(deletedMessages);
            await _context.SaveChangesAsync();
        }
    }
}
