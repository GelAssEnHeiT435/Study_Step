using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;

namespace Study_Step_Server.Repositories
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        public FileRepository(ApplicationContext context) : base(context) { }
    }
}
