namespace MtSmart.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICardRepository Cards { get; }
        IColumnRepository Columns { get; }
        ICommentRepository Comments { get; }
        IFileRepository Files { get; }
        IUpdateRepository Updates { get; }
        IUserRepository Users { get; }
        IAssessmentQualityResultRepository AssessmentQualityResults { get; }
        IAssessmentTermResultRepository AssessmentTermResults { get; }
        IMailRepository Mails { get; }

        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
