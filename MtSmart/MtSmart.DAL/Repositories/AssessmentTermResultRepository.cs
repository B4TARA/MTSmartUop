using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class AssessmentTermResultRepository : RepositoryBase<AssessmentTermResult>, IAssessmentTermResultRepository
    {
        public AssessmentTermResultRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
