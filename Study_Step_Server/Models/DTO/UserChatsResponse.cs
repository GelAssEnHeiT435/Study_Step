namespace Study_Step_Server.Models.DTO
{
    public class UserChatsResponse
    {
        public IEnumerable<ChatDTO> Chats { get; set; }
        public IEnumerable<UserChatDTO> UserChats { get; set; }
    }
}
