using MtSmart.DAL.Interfaces;
using File = MtSmart.DAL.Entities.File;

namespace MtSmart.DAL.Repositories
{
    public class FileRepository : RepositoryBase<File>, IFileRepository
    {
        public FileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
