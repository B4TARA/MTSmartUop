using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
