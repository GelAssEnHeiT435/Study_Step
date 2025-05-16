using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;

namespace Study_Step_Server.Repositories
{
    public class DeletedChatRepository : Repository<DeletedChat>, IDeletedChatRepository
    {
        public DeletedChatRepository(ApplicationContext context) : base(context) { }

        public async Task DeleteChatForUserAsync(int userId, int chatId)
        {
            var existingRecord = await _context.DeletedChats
                .FirstOrDefaultAsync(dc => dc.UserId == userId && dc.ChatId == chatId);

            if (existingRecord == null)
            {
                _context.DeletedChats.Add(new DeletedChat
                {
                    UserId = userId,
                    ChatId = chatId,
                    DeletedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RestoreChatAsync(int senderId, int chatId)
        {
            var recipientIds = await _context.UserChats
                .Where(uc => uc.ChatId == chatId)
                .Select(uc => uc.UserId)
                .ToListAsync();

            foreach (var recipientId in recipientIds)
            {
                var deletedChat = await _context.DeletedChats
                    .FirstOrDefaultAsync(dc => dc.UserId == recipientId && dc.ChatId == chatId);

                if (deletedChat != null)
                {
                    _context.DeletedChats.Remove(deletedChat);
                }
            }

            var chat = await _context.Chats.FindAsync(chatId);
            chat.LastMessageTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
