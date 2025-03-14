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
            #region Converter Files

            CreateMap<FileModel, FileModelDTO>()
                .ForMember(dest => dest.FileBytes, opt => opt.MapFrom<FileConvertResolver<FileModel, FileModelDTO>>());
            CreateMap<FileModelDTO, FileModel>();

            #endregion

            #region Converter Messages

            // TODO: Add converting files
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files));
            CreateMap<MessageDTO, Message>();

            #endregion

            #region Converter Users

            CreateMap<User, UserDTO>();

            _ = CreateMap<UserDTO, User>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom<ImageConvertResolver<UserDTO, User>>());

            #endregion

            #region Converter UserChat

            CreateMap<UserChat, UserChatDTO>();
            CreateMap<UserChatDTO, UserChat>();

            #endregion

            #region Converter Chat

            CreateMap<Chat, ChatDTO>();
            CreateMap<ChatDTO, Chat>()
                .ForMember(dest => dest.bitmapPhoto, opt => opt.MapFrom<ImageConvertResolver<ChatDTO, Chat>>());

            #endregion
        }
    }
}