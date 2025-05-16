using Study_Step_Server.Models;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Study_Step_Server.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<Message>> GetMessagesByChatIdAsync(int userId, int chatId)
        {
            // 1. Проверяем, удалён ли чат для пользователя
            bool isChatDeletedForUser = await _context.DeletedChats
                .AnyAsync(dc => dc.UserId == userId && dc.ChatId == chatId);

            // Если чат удалён — возвращаем пустой список
            if (isChatDeletedForUser) return new List<Message>();

            // 2. Получаем ID удалённых сообщений (для пользователя или всех)
            var deletedMessageIds = await _context.DeletedMessages
                .Where(dm => dm.UserId == userId || dm.IsDeletedForEveryone)
                .Select(dm => dm.MessageId)
                .ToListAsync();

            // 3. Возвращаем только неудалённые сообщения
            return await _context.Messages
                .Include(m => m.Files)  // Подгружаем файлы, если нужно
                .Where(m => m.ChatId == chatId)
                .Where(m => !deletedMessageIds.Contains(m.MessageId))
                .OrderBy(m => m.SentAt)  // Сортируем по времени отправки (если нужно)
                .ToListAsync();
        }

        public async Task<Message?> GetLastUndeletedMessageAsync(int chatId, int userId)
        {
            return await _dbSet.Include(m => m.Files)
                               .Where(m => m.ChatId == chatId)
                               .Where(m => !_context.DeletedMessages
                                   .Any(dm => dm.MessageId == m.MessageId &&
                                             (dm.UserId == userId || dm.IsDeletedForEveryone)))
                               .OrderByDescending(m => m.SentAt)
                               .FirstOrDefaultAsync();
        }

        public async Task UpdateMessageWithFiles(Message updatedMessage)
        {
            var existingMessage = await _dbSet.Include(m => m.Files)
                                              .FirstOrDefaultAsync(m => m.MessageId == updatedMessage.MessageId);

            if (existingMessage == null) return;

            _context.Entry(existingMessage).CurrentValues.SetValues(updatedMessage);

            await SyncFiles(existingMessage, updatedMessage.Files?.ToList() ?? new List<FileModel>());
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadMessagesCount(int chatId, int userId)
        {

            return await _dbSet.Where(m => m.ChatId == chatId)
                               .Where(m => m.UserId != userId)
                               .Where(m => !m.ReadByUsers.Any(r => r.UserId == userId))
                               .Where(m => m.SentAt > (_context.UserChats
                                    .Where(uc => uc.ChatId == chatId && uc.UserId == userId)
                                    .Select(uc => uc.LastReadTime)
                                    .FirstOrDefault() ?? DateTime.MinValue))
                               .CountAsync();
        }

        public async Task MarkAllAsRead(int chatId, int userId)
        {
            var currentTime = DateTime.UtcNow;

            // Search unread messages
            var unreadMessages = await _dbSet.Where(m => m.ChatId == chatId)
                                             .Where(m => m.UserId != userId)
                                             .Where(m => !m.ReadByUsers.Any(r => r.UserId == userId))
                                             .ToListAsync();
            // Add reading messages
            foreach (var message in unreadMessages) {
                _context.ReadMessages.Add(new MessageRead() {
                    MessageId = message.MessageId,
                    UserId = userId,
                    ReadAt = currentTime
                });
            }

            // Update last time reading the message
            var userChat = await _context.UserChats
                .FirstOrDefaultAsync(uc => uc.ChatId == chatId && uc.UserId == userId);

            if (userChat != null) userChat.LastReadTime = currentTime;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> isReadMessageByIdAsync(int messageId)
        {
            return _context.ReadMessages.Any(rm => rm.MessageId == messageId);
        }

        private async Task SyncFiles(Message existingMessage, List<FileModel> updatedFiles)
        {
            var existingFiles = existingMessage.Files?.ToList() ?? new List<FileModel>();
            var filesToRemove = existingFiles.Where(ef => !updatedFiles.Any(uf => uf.FileModelId == ef.FileModelId && ef.FileModelId != 0)).ToList();

            foreach (var file in filesToRemove){
                _context.Files.Remove(file);
            }

            var newFiles = updatedFiles.Where(uf => uf.FileModelId == 0).ToList();

            foreach (var newFile in newFiles) {
                var fileToAdd = new FileModel
                {
                    Name = newFile.Name,
                    Extension = newFile.Extension,
                    Size = newFile.Size,
                    MimeType = newFile.MimeType,
                    Path = newFile.Path,
                    CreatedAt = DateTime.UtcNow,
                    MessageId = existingMessage.MessageId
                };
                existingMessage.Files.Add(fileToAdd);
            }

            var filesToUpdate = updatedFiles
                .Where(uf => uf.FileModelId != 0 && existingFiles.Any(ef => ef.FileModelId == uf.FileModelId))
                .ToList();

            foreach (var updatedFile in filesToUpdate){
                var existingFile = existingFiles.First(ef => ef.FileModelId == updatedFile.FileModelId);
                _context.Entry(existingFile).CurrentValues.SetValues(updatedFile);
            }
        }
    }
}