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
            #region File

            // TODO: Converting file to bytes and forth
            CreateMap<FileModel, FileModelDTO>()
                .ForMember(dest => dest.FileBytes, opt => opt.MapFrom<FileConvertResolver<FileModel, FileModelDTO>>());
            CreateMap<FileModelDTO, FileModel>()
                .ForMember(dest => dest.Path, opt => opt.MapFrom<FileModelPathResolver<FileModelDTO, FileModel>>());

            #endregion

            #region Converter Messages

            // TODO: Add converting files
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files));
            CreateMap<MessageDTO, Message>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files));

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