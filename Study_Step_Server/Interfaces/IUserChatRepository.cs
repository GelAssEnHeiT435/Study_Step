﻿using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IUserChatRepository : IRepository<UserChat>
    {
        Task<List<Chat>> GetChatsByUserIdAsync(int userId); // get all user's chats
        Task<User?> FindSecondUserAsync(int chatId, int userId); // get second user in chat
        Task<List<UserChat>> GetLinksByChatIdsAsync(List<Chat?> chats);
    }
}
    