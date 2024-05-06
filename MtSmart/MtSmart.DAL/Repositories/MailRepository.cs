using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class MailRepository : RepositoryBase<Mail>, IMailRepository
    {
        public MailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
