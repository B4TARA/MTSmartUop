using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class UpdateRepository : RepositoryBase<Update>, IUpdateRepository
    {
        public UpdateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
