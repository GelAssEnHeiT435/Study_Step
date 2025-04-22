using Study_Step_Server.Models;
using Study_Step_Server.Repositories;

namespace Study_Step_Server.Interfaces
{
    public interface IChatRepository : IRepository<Chat>
    {
        Task<Chat?> SearchIndividualChatAsync(int clientId, int companionId);
    }
}
