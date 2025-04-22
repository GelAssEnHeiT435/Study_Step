using AutoMapper;
using Study_Step.Interfaces;
using System.Collections;
using System.Windows.Media.Imaging;

namespace Study_Step.Data.Resolvers
{
    public class ImageConvertResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, BitmapImage?> 
        where TSource : class
        where TDestination : class
    {
        private readonly IFileService _fileService;
        public ImageConvertResolver(IFileService fileService)
        {
            _fileService = fileService;
        }

        public BitmapImage? Resolve(TSource source, TDestination destination, BitmapImage? destMember, ResolutionContext context)
        {
            byte[]? imageByte = source.GetType().GetProperty("ContactPhoto")?.GetValue(source) as byte[];
            
            if (imageByte != null && imageByte.Length == 0) { return null; }
            return _fileService.ConvertByteArrayToBitmapImage(imageByte);
        }
    }
}