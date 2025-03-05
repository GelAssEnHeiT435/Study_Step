using AutoMapper;
using Study_Step_Server.Data.Resolvers;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;
using Study_Step_Server.Models.DTO;
using Study_Step_Server.Services;

namespace Study_Step_Server.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            #region Converter Messages

            // TODO: Add converting files
            CreateMap<Message, MessageDTO>();
            CreateMap<MessageDTO, Message>();

            #endregion

            #region Converter Users

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.ContactPhoto, opt => opt.MapFrom<ImageConvertResolver<User, UserDTO>>());

            CreateMap<UserDTO, User>(); // TODO: add convert byte array to file and get URL

            #endregion

            #region UserChat

            CreateMap<UserChat, UserChatDTO>();
            CreateMap<UserChatDTO, UserChat>();

            #endregion

            #region Chat

            CreateMap<Chat, ChatDTO>();
            CreateMap<ChatDTO, Chat>();

            #endregion
        }
    }
}