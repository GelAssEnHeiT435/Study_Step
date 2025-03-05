using AutoMapper;
using Study_Step.Interfaces;
using System.Windows.Media.Imaging;

namespace Study_Step.Data.Resolvers
{
    public class ImageConvertResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, BitmapImage?> 
        where TSource : class
        where TDestination : class
    {
        private readonly IImageService _imageService;
        public ImageConvertResolver(IImageService imageService)
        {
            _imageService = imageService;
        }

        public BitmapImage? Resolve(TSource source, TDestination destination, BitmapImage? destMember, ResolutionContext context)
        {
            byte[]? imageByte = source.GetType().GetProperty("ContactPhoto")?.GetValue(source) as byte[];
            return _imageService.ConvertByteArrayToBitmapImage(imageByte);
        }
    }
}