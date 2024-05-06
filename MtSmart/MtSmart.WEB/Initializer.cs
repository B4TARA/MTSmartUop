using MtSmart.BLL.Interfaces;
using MtSmart.BLL.Services;
using MtSmart.DAL.Interfaces;
using MtSmart.DAL.Repositories;

namespace MtSmart.WEB
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAssessmentQualityResultRepository, AssessmentQualityResultRepository>();
            services.AddScoped<IAssessmentTermResultRepository, AssessmentTermResultRepository>();
            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<IColumnRepository, ColumnRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IMailRepository, MailRepository>();
            services.AddScoped<IUpdateRepository, UpdateRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<ISupervisorService, SupervisorService>();
            services.AddScoped<IUserBoardService, UserBoardService>();
        }
    }
}
