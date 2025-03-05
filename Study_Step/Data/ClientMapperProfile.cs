using AutoMapper;
using Study_Step.Data.Resolvers;
using Study_Step.Models;
using Study_Step.Models.DTO;

namespace Study_Step.Data
{
    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile() 
        {
            #region Converter Messages

            // TODO: Add converting files
            CreateMap<Message, MessageDTO>();
            CreateMap<MessageDTO, Message>();

            #endregion

            #region Converter Users

            CreateMap<User, UserDTO>();
            
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom<ImageConvertResolver<UserDTO, User>>());

            #endregion

            #region UserChat

            CreateMap<UserChat, UserChatDTO>();
            CreateMap<UserChatDTO, UserChat>();

            #endregion

            #region Chat

            CreateMap<Chat, ChatDTO>();
            CreateMap<ChatDTO, Chat>()
                .ForMember(dest => dest.bitmapPhoto, opt => opt.MapFrom<ImageConvertResolver<ChatDTO, Chat>>());

            #endregion
        }
    }
}