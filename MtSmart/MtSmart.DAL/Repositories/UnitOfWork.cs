using MtSmart.DAL.Interfaces;

namespace MtSmart.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private ICardRepository? cardRepository;
        private IColumnRepository? columnRepository;
        private ICommentRepository? commentRepository;
        private IFileRepository? fileRepository;
        private IUpdateRepository? updateRepository;
        private IUserRepository? userRepository;
        private IAssessmentQualityResultRepository? assessmentQualityResultRepository;
        private IAssessmentTermResultRepository? assessmentTermResultRepository;
        private IMailRepository? mailRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICardRepository Cards
        {
            get
            {
                if (cardRepository == null)
                    cardRepository = new CardRepository(_dbContext);
                return cardRepository;
            }
        }

        public IColumnRepository Columns
        {
            get
            {
                if (columnRepository == null)
                    columnRepository = new ColumnRepository(_dbContext);

                return columnRepository;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_dbContext);

                return userRepository;
            }
        }

        public ICommentRepository Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(_dbContext);

                return commentRepository;
            }
        }

        public IFileRepository Files
        {
            get
            {
                if (fileRepository == null)
                    fileRepository = new FileRepository(_dbContext);

                return fileRepository;
            }
        }

        public IUpdateRepository Updates
        {
            get
            {
                if (updateRepository == null)
                    updateRepository = new UpdateRepository(_dbContext);

                return updateRepository;
            }
        }

        public IAssessmentQualityResultRepository AssessmentQualityResults
        {
            get
            {
                if (assessmentQualityResultRepository == null)
                    assessmentQualityResultRepository = new AssessmentQualityResultRepository(_dbContext);

                return assessmentQualityResultRepository;
            }
        }

        public IAssessmentTermResultRepository AssessmentTermResults
        {
            get
            {
                if (assessmentTermResultRepository == null)
                    assessmentTermResultRepository = new AssessmentTermResultRepository(_dbContext);

                return assessmentTermResultRepository;
            }
        }

        public IMailRepository Mails
        {
            get
            {
                if (mailRepository == null)
                    mailRepository = new MailRepository(_dbContext);

                return mailRepository;
            }
        }


        public void Commit()
             => _dbContext.SaveChanges();
        public async Task CommitAsync()
            => await _dbContext.SaveChangesAsync();
        public void Rollback()
            => _dbContext.Dispose();


        public async Task RollbackAsync()
            => await _dbContext.DisposeAsync();


        private bool disposed = false;


        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }


                disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
