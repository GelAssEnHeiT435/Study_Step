using AutoMapper;
using Study_Step.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_Step.Data.Resolvers
{
    public class FileConvertResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, byte[]?>
    {
        private readonly IFileService _fileService;
        public FileConvertResolver(IFileService fileService)
        {
            _fileService = fileService;
        }

        public byte[]? Resolve(TSource source, TDestination destination, byte[]? destMember, ResolutionContext context)
        {
            string? filepath = source.GetType().GetProperty("Path")?.GetValue(source) as string;

            if (filepath == null) { return null; }

            return _fileService.ConvertFileToByteArray(filepath);
        }
    }
}
