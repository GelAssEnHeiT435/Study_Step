using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Data;
using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models.DTO;
using System.Linq;

namespace Study_Step_Server.Repositories
{
    public class UserChatRepository : Repository<UserChat>, IUserChatRepository
    {
        public UserChatRepository(ApplicationContext context) : base(context) { }

        public async Task<List<Chat>> GetChatsByUserIdAsync(int userId)
        {
            return await _dbSet.Include(uc => uc.Chat) 
                                   .ThenInclude(c => c.Messages)
                               .Where(uc => uc.UserId == userId && uc.Chat.Messages.Any()) 
                               .OrderByDescending(uc => uc.Chat.LastMessageTime)
                               .Select(uc => uc.Chat) 
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
