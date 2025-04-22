using AutoMapper;
using Study_Step.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Study_Step.Data.Resolvers
{
    public class ByteImageConvertResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, byte[]?>
        where TSource : class
        where TDestination : class
    {
        private readonly IFileService _fileService;
        public ByteImageConvertResolver(IFileService fileService)
        {
            _fileService = fileService;
        }

        public byte[]? Resolve(TSource source, TDestination destination, byte[]? destMember, ResolutionContext context)
        {
            BitmapImage? img = source.GetType().GetProperty("bitmapPhoto")?.GetValue(source) as BitmapImage;

            if (img == null) return null;
            return _fileService.ConvertBitmapImageToByteArray(img);
        }
    }
}
