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

            CreateMap<FileModel, FileModelDTO>();
            CreateMap<FileModelDTO, FileModel>();

            CreateMap<FileModel, DownloadItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FileModelId))
                .ForMember(dest => dest.SavePath, opt => opt.MapFrom(src => src.Path));

            #endregion

            #region Converter Messages

            // TODO: Add converting files
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.Files));
            CreateMap<MessageDTO, Message>();

            #endregion

            #region Converter Users

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.ContactPhoto, opt => opt.MapFrom<ByteImageConvertResolver<User, UserDTO>>());
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.bitmapPhoto, opt => opt.MapFrom<ImageConvertResolver<UserDTO, User>>());

            #endregion

            #region Converter UserChat

            CreateMap<UserChat, UserChatDTO>();
            CreateMap<UserChatDTO, UserChat>();

            #endregion

            #region Converter Chat

            CreateMap<Chat, ChatDTO>()
                .ForMember(dest => dest.ContactPhoto, opt => opt.MapFrom<ByteImageConvertResolver<Chat, ChatDTO>>());
            CreateMap<ChatDTO, Chat>()
                .ForMember(dest => dest.bitmapPhoto, opt => opt.MapFrom<ImageConvertResolver<ChatDTO, Chat>>());

            #endregion
        }
    }
}