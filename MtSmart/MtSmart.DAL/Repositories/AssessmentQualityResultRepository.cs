using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class AssessmentQualityResultRepository : RepositoryBase<AssessmentQualityResult>, IAssessmentQualityResultRepository
    {
        public AssessmentQualityResultRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
