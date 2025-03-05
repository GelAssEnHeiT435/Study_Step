using Study_Step_Server.Data;
using Study_Step_Server.Interfaces;
using Study_Step_Server.Models;

namespace Study_Step_Server.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }
    }
}

