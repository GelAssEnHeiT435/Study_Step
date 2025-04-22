using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;

namespace Study_Step_Server.Repositories
{
    public class ChatRepository : Repository<Chat>, IChatRepository
    {
        public ChatRepository(ApplicationContext context) : base(context) { }

        public async Task<Chat?> SearchIndividualChatAsync(int clientId, int companionId)
        {
            return await _dbSet.Include(c => c.UserChats)
                               .Where(c => c.Type == ChatType.Individual)
                               .FirstOrDefaultAsync(c => c.UserChats.Any(uc => uc.UserId == clientId) &&
                                                         c.UserChats.Any(uc => uc.UserId == companionId));
        }
    }
}
