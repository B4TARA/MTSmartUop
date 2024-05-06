using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
