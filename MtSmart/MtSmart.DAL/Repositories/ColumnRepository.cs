using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class ColumnRepository : RepositoryBase<Column>, IColumnRepository
    {
        public ColumnRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
