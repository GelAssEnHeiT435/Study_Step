using AutoMapper;
using AutoMapper.Execution;
using Study_Step_Server.Interfaces;

namespace Study_Step_Server.Data.Resolvers
{
    public class ImageConvertResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, byte[]?> 
        where TSource : class
        where TDestination : class
    {
        private readonly IImageService _imageService;
        public ImageConvertResolver(IImageService imageService)
        {
            _imageService = imageService;
        }

        public byte[]? Resolve(TSource source, TDestination destination, byte[]? destMember, ResolutionContext context)
        {
            string? imagePath = source.GetType().GetProperty("ContactPhoto")?.GetValue(source) as string;
            return _imageService.ConvertImageToByteArray(imagePath);
        }
    }
}