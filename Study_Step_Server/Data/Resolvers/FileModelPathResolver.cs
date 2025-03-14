using AutoMapper;
using Study_Step_Server.Interfaces;

namespace Study_Step_Server.Data.Resolvers
{
    public class FileModelPathResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string?>
    {
        private readonly IFileService _fileService;
        public FileModelPathResolver(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string? Resolve(TSource source, TDestination destination, string? destMember, ResolutionContext context)
        {
            byte[]? fileBytes = source?.GetType().GetProperty("FileBytes")?.GetValue(source) as byte[];
            string? name = source?.GetType().GetProperty("Name")?.GetValue(source) as string;

            if (fileBytes == null) { return null; }

            return _fileService.SaveFile(fileBytes, name);
        }
    }
}
