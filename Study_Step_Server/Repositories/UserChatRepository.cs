using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Repositories
{
    public class UserChatRepository : Repository<UserChat>, IUserChatRepository
    {
        public UserChatRepository(ApplicationContext context) : base(context) { }

        public async Task<List<Chat>> GetChatsByUserIdAsync(int userId)
        {
            var deletedChatIds = await _context.DeletedChats
                .Where(dc => dc.UserId == userId)
                .Select(dc => dc.ChatId)
                .ToListAsync();

            var deletedMessageIds = await _context.DeletedMessages
                .Where(dm => dm.UserId == userId || dm.IsDeletedForEveryone)
                .Select(dm => dm.MessageId)
                .Distinct()
                .ToListAsync();

            return await _context.UserChats
                .Where(uc => uc.UserId == userId && !deletedChatIds.Contains(uc.ChatId))
                .Select(uc => uc.Chat)
                .Where(c => c.Messages.Any(m => !deletedMessageIds.Contains(m.MessageId)))
                .OrderByDescending(c => c.LastMessageTime)
                .ToListAsync();
        }

        public async Task<User?> FindSecondUserAsync(int chatId, int userId)
        {
            return await _dbSet.Include(uc => uc.User)
                                .Where(uc => uc.Chat.Type == ChatType.Individual &&
                                             uc.ChatId == chatId && uc.UserId != userId)
                                .Select(uc => uc.User)
                                .FirstOrDefaultAsync();
        }

        public async Task<List<UserChat>> GetLinksByChatIdsAsync(List<Chat?> chats)
        {
            var chatIds = chats.Select(chat => chat.ChatId).ToList();
            return await _dbSet
                .Where(uc => chatIds.Contains(uc.ChatId)) 
                .ToListAsync(); 
        }
    }
}
