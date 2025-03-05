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

        public async Task<IEnumerable<Chat>> GetChatsByUserIdAsync(int userId)
        {
            return await _dbSet.Include(uc => uc.Chat) 
                               .Where(uc => uc.UserId == userId) 
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

        public async Task<IEnumerable<UserChat?>> GetLinksByChatIdsAsync(IEnumerable<ChatDTO?> dtoChats)
        {
            var chatIds = dtoChats.Select(chat => chat.ChatId).ToList();

            return await _dbSet
                .Where(uc => chatIds.Contains(uc.ChatId)) 
                .ToListAsync(); 
        }
    }
}
