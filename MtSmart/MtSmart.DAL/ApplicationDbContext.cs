using Microsoft.EntityFrameworkCore;
using MtSmart.DAL.Entities;
using MtSmart.DAL.Enums;
using File = MtSmart.DAL.Entities.File;

namespace MtSmart.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<AssessmentQualityResult> AssessmentQualityResults {  get; set; }
        public DbSet<AssessmentTermResult> AssessmentTermResults { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mail>(builder =>
            {
                // Выставление сотрудниками оценочных суждений и заполнение новых задач
                builder.HasData(new Mail[]
                {
                    new Mail
                    {
                        Id = 1,
                        Code = MailCodes.EmployeeQuarterlySmartTaskSummary,
                        Title = "Уведомление",
                        Body = "<div>\r\n    Пришло время подвести <b>итоги реализации SMART-задач</b> и заполнить <b>SMART-задачи на следующий месяц.</b>\r\n    <span>Вы можете сделать это по ссылке </span>\r\n    <a href=\"https://10.117.11.77:80/Account/LogOut\" target=\"_blank\">https://10.117.11.77:80/Account/LogOut</a>\r\n    <br>\r\n    <div>или через ярлык на рабочем столе</div>\r\n    <div>\r\n        <span style=\"width:50px; height:50px;\">\r\n            <img style=\"width:50px; height:50px;\" src='cid:{0}'>\r\n        </span>\r\n    </div>\r\n    <br>\r\n    <div>\r\n        Просим Вас указать оценочные суждения по задачам и заполнить SMART-задачи на следующий месяц.\r\n    </div>\r\n</div>\r\n",
                    },
                });

                builder.HasData(new Mail[]
                {
                    new Mail
                    {
                        Id = 2,
                        Code = MailCodes.NextQuarterTaskAssignment,
                        Title = "Напоминание",
                        Body = "<div>\r\n    Напоминаем о необходимости подвести <b>итоги реализации SMART-задач</b> и заполнить <b>SMART-задачи на следующий месяц.</b>\r\n    <span>Вы можете сделать это по ссылке </span>\r\n    <a href=\"https://10.117.11.77:80/Account/LogOut\" target=\"_blank\">https://10.117.11.77:80/Account/LogOut</a>\r\n    <br>\r\n    <div>или через ярлык на рабочем столе</div>\r\n    <div>\r\n        <span style=\"width:50px; height:50px;\">\r\n            <img style=\"width:50px; height:50px;\" src='cid:{0}'>\r\n        </span>\r\n    </div>\r\n    <br>\r\n    <div>\r\n        Просим Вас указать оценочные суждения по задачам и заполнить SMART-задачи на следующий месяц.\r\n    </div>\r\n</div>\r\n",
                    },
                });

                // Выставление руководителями оценочных суждений и согласование новых задач
                builder.HasData(new Mail[]
                {
                    new Mail
                    {
                        Id = 3,
                        Code = MailCodes.SupervisorQuarterlySmartTaskSummary,
                        Title = "Уведомление",
                        Body = "<div>\r\n    Пришло время подвести <b>итоги реализации SMART-задач Ваших работников</b> и согласовать <b>новые SMART-задачи.</b>\r\n    <span> Вы можете сделать это по ссылке </span>\r\n    <a href=\"https://10.117.11.77:80/Account/LogOut\" target=\"_blank\">https://10.117.11.77:80/Account/LogOut</a>\r\n    <br>\r\n    <div>или через ярлык на рабочем столе</div>\r\n    <div>\r\n        <span style=\"width:50px; height:50px;\">\r\n            <img style=\"width:50px; height:50px;\" src='cid:{0}'>\r\n        </span>\r\n    </div>\r\n    <br>\r\n    <div>\r\n        Просим Вас согласовать оценочные суждения по задачам Ваших работников и согласовать новые SMART-задачи\r\n    </div>\r\n</div>",
                    },
                });

                // Задача была отправлена на доработку
                builder.HasData(new Mail[]
                {
                    new Mail
                    {
                        Id = 4,
                        Code = MailCodes.TaskSentForRevision,
                        Title = "Уведомление",
                        Body = "\"<div>\" +\r\n                \"Информируем, что Ваша задача отправлена на доработку и находится на этапе \\\"Составление SMART-задач работником.\\\"\" +\r\n                \"<br>\" +\r\n                \"Редактирование SMART-задач доступно по ссылке:\" +\r\n                    \"<a href= \\\"https://10.117.11.77:80/Account/LogOut\\\" target = \\\"blanc\\\">Посмотреть задачу можно по ссылке<a/>\" +\r\n                    \"<br>\" +\r\n                    \"<div>\"+\"или через ярлык на рабочем столе\"+\"<div>\" +\r\n                    \"<span style=\\\"width:50px; height:50px;\\\">\" +\r\n                            \"<img style=\\\"width:50px; height:50px;\\\" src='cid:{0}'>\" +\r\n                    \"</span>\" +\r\n            \"</div>\"",
                    },
                });
            });

            modelBuilder.Entity<AssessmentQualityResult>(builder =>
            {
                builder.HasData(new AssessmentQualityResult[]
                {
                    new AssessmentQualityResult
                    {
                        Id = 1,
                        Value = "91-100%",
                        Text = "Задача выполнена в полном объеме, качество задачи абсолютно соответствует поставленой цели и критериям ее реализации. Имеющиеся отклонения от первоначальной сути согласованы должным образом своевременно.",
                        Description = "91-100%",
                    },

                    new AssessmentQualityResult
                    {
                        Id = 2,
                        Value = "81-90%",
                        Text = "Задача выполнена в полном объеме, в задаче есть незначительные отклонения от первоначальных критериев, не нарушающие концептуальную суть идеи и не влияющие на удобство пользователя или производительность процесса.",
                        Description = "81-90%",
                    },

                    new AssessmentQualityResult
                    {
                        Id = 3,
                        Value = "71-80%",
                        Text = "Задача реализована, отклонения от первоначальных критериев есть. В целом суть идеи реализована, отдельные элементы требуют доработки в последующем.ы",
                        Description = "71-80%",
                    },

                    new AssessmentQualityResult
                    {
                        Id = 4,
                        Value = "50-70%",
                        Text = "Задача реализована частично, значительная часть критериев не соответствует запросу или не проработана, требуется существенная доработка.",
                        Description = "50-70%",
                    },

                    new AssessmentQualityResult
                    {
                        Id = 5,
                        Value = "менее 50%",
                        Text = "Задача считается выполненной некачественно, выполненной с отклонениями от изначальных критериев, невыполненной.",
                        Description = "менее 50%",
                    },                  
                });
            });

            modelBuilder.Entity<AssessmentTermResult>(builder =>
            {
                builder.HasData(new AssessmentTermResult[]
                {
                    new AssessmentTermResult
                    {
                        Id = 1,
                        Value = "100%",
                        Text = "Выполнено в согласованный срок / ранее согласованного срока.",
                        Description = "100%",
                    },

                    new AssessmentTermResult
                    {
                        Id = 2,
                        Value = "90-99%",
                        Text = "Выполнено с переносом срока, перенос срока заранее согласован.",
                        Description = "90-99%",
                    },

                    new AssessmentTermResult
                    {
                        Id = 3,
                        Value = "50-80%",
                        Text = "Выполнено с переносом срока, перенос срока согласован в день в день сдачи задачи (итоговая оценка зависит от приоритета и важности задачи).",
                        Description = "50-80%",
                    },

                    new AssessmentTermResult
                    {
                        Id = 4,
                        Value = "менее 50%",
                        Text = "Выполнено с нарушением срока (нарушением срока считается любая задержка сдачи задачи без предварительного согласования с руководителем, ставившим задачу), не выполнено или выполнено частично к установленому сроку.",
                        Description = "менее 50%",
                    },                               
                });
            });
        }
    }
}
