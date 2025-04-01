using AutoMapper;
using Study_Step.Interfaces;
using Study_Step.Models;
using Study_Step.Models.DTO;

namespace Study_Step.Services
{
    public class DtoConverterService
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public DtoConverterService(IMapper mapper, IFileService fileService)
        {
            _mapper = mapper;
            _fileService = fileService;
        }

        #region Messages
        public MessageDTO GetMessageDTO(Message message) =>
            _mapper.Map<MessageDTO>(message);

        public IEnumerable<MessageDTO> GetMessageListDTO( IEnumerable<Message> messages ) => 
            _mapper.Map<IEnumerable<MessageDTO>>(messages);

        public Message GetMessage(MessageDTO message) =>
            _mapper.Map<Message>(message);

        public IEnumerable<Message> GetMessageList( IEnumerable<MessageDTO> messages ) =>
            _mapper.Map<IEnumerable<Message>>(messages);
        #endregion

        #region Users
        public UserDTO GetUserDTO(User user) =>
            _mapper.Map<UserDTO>(user);

        public IEnumerable<UserDTO> GetUserListDTO(IEnumerable<User> users) =>
            _mapper.Map<IEnumerable<UserDTO>>(users);

        public User GetUser(UserDTO user) =>
            _mapper.Map<User>(user);

        public IEnumerable<User> GetUserList( IEnumerable<UserDTO> users) =>
            _mapper.Map<IEnumerable<User>>(users);
        #endregion

        #region UserChats
        public UserChatDTO GetUserChatDTO(UserChat userchat) =>
            _mapper.Map<UserChatDTO>(userchat);

        public IEnumerable<UserChatDTO> GetUserChatListDTO(IEnumerable<UserChat> userchats) =>
            _mapper.Map<IEnumerable<UserChatDTO>>(userchats);

        public UserChat GetUserChat(UserChatDTO userchat) =>
            _mapper.Map<UserChat>(userchat);

        public IEnumerable<UserChat> GetUserChatList( IEnumerable<UserChatDTO> userchats ) =>
            _mapper.Map<IEnumerable<UserChat>>(userchats);
        #endregion

        #region Chats

        public ChatDTO GetChatDTO(Chat chat) =>
            _mapper.Map<ChatDTO>(chat);

        public IEnumerable<ChatDTO> GetChatListDTO( IEnumerable<Chat> chats ) =>
            _mapper.Map<IEnumerable<ChatDTO>>(chats);

        public Chat GetChat(ChatDTO chat) =>
            _mapper.Map<Chat>(chat);

        public IEnumerable<Chat> GetChatList( IEnumerable<ChatDTO> chats ) =>
            _mapper.Map<IEnumerable<Chat>>(chats);

        #endregion

        #region Files
        public FileModelDTO GetFileDTO(FileModel chat) =>
            _mapper.Map<FileModelDTO>(chat);

        public IEnumerable<FileModelDTO> GetFileListDTO(IEnumerable<FileModel> chats) =>
            _mapper.Map<IEnumerable<FileModelDTO>>(chats);

        public FileModel GetFile(FileModelDTO chat) =>
            _mapper.Map<FileModel>(chat);

        public IEnumerable<FileModel> GetFileList(IEnumerable<FileModelDTO> chats) =>
            _mapper.Map<IEnumerable<FileModel>>(chats);

        public DownloadItem GetDownloadItem(FileModel file) =>
            _mapper.Map<DownloadItem>(file);

        #endregion
    }
}
