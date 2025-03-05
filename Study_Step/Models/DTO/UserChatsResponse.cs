using Study_Step.Models;

namespace Study_Step.Models.DTO
{
    public class UserChatsResponse
    {
        public IEnumerable<ChatDTO> Chats { get; set; }
        public IEnumerable<UserChatDTO> UserChats { get; set; }
    }
}
