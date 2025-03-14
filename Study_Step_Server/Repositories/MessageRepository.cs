using Study_Step_Server.Models;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int chatId)
        {
            return await _dbSet.Include(m => m.Files)
                               .Where(m => m.ChatId == chatId)
                               .ToListAsync();
        }
    }
}