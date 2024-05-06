using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using MtSmart.DAL;
using MtSmart.DAL.Entities;
using MtSmart.DAL.Enums;
using MtSmart.DAL.Repositories;
using MtSmart.Import.Utils;
using Newtonsoft.Json.Linq;
using NLog;
using System.Xml;

namespace MtSmart.Import
{
    public class ExportAndImport
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ExportAndImport(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public async Task TransferUsersFromExcelToDatabase(string usersPath, string colsArrayPath, string usersPassContainerPath, string userImgDownloadPath)
        {
            try
            {
                var usersFromExcel = ReadUsersFromExcel(usersPath);
                PopulateUsersWithXmlData(usersFromExcel, colsArrayPath, usersPassContainerPath, userImgDownloadPath);
                await UpdateOrCreateUsersInDatabase(usersFromExcel);
                await CheckForNotifications(usersFromExcel);
                await BlockNonActiveUsers(usersFromExcel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }

        private List<User> ReadUsersFromExcel(string usersPath)
        {
            var usersFromExcel = new List<User>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var fStream = System.IO.File.Open(usersPath, FileMode.Open, FileAccess.Read))
            {
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fStream);
                var resultDataSet = excelDataReader.AsDataSet();
                var table = resultDataSet.Tables[0];

                // Добавление непосредственных руководителей
                for (int rowCounter = 2; rowCounter < table.Rows.Count; rowCounter++)
                {
                    var supervisorName = Convert.ToString(table.Rows[rowCounter][4]);

                    // Проверка наличия имени руководителя и его уникальности в списке
                    if (!string.IsNullOrEmpty(supervisorName) && usersFromExcel.All(u => u.Name != supervisorName))
                    {
                        usersFromExcel.Add(new User
                        {
                            Name = supervisorName,
                            Role = Roles.Supervisor
                        });
                    }
                }

                // Добавление сотрудников
                for (int rowCounter = 2; rowCounter < table.Rows.Count; rowCounter++)
                {
                    var userFromExcel = new User
                    {
                        Name = Convert.ToString(table.Rows[rowCounter][1]),
                        Position = Convert.ToString(table.Rows[rowCounter][2]),
                        SspName = Convert.ToString(table.Rows[rowCounter][3]),
                        SupervisorName = Convert.ToString(table.Rows[rowCounter][4])
                    };

                    // Проверка, был ли пользователь уже добавлен как руководитель
                    var existingUser = usersFromExcel.FirstOrDefault(u => u.Name == userFromExcel.Name);
                    if (existingUser == null)
                    {
                        userFromExcel.Role = Roles.Employee;
                        usersFromExcel.Add(userFromExcel);
                    }
                    else
                    {
                        existingUser.Position = userFromExcel.Position;
                        existingUser.SspName = userFromExcel.SspName;
                        existingUser.SupervisorName = userFromExcel.SupervisorName;
                        existingUser.Role = Roles.Combined;
                    }
                }

                excelDataReader.Close();
            }

            return usersFromExcel;
        }

        private void PopulateUsersWithXmlData(List<User> usersFromExcel, string colsArrayPath, string usersPassContainerPath, string userImgDownloadPath)
        {
            var xmlDocument1 = new XmlDocument();
            xmlDocument1.Load(colsArrayPath);

            var xmlDocument2 = new XmlDocument();
            xmlDocument2.Load(usersPassContainerPath);

            foreach (var userFromExcel in usersFromExcel)
            {
                var parts = userFromExcel.Name.Split(' ');
                var firstName = parts[0];
                var lastName = parts.Length > 1 ? parts[1] : "";
                var middleName = parts.Length > 2 ? parts[2] : "";

                var xpath = $"//value[contains(., '{firstName}') and contains(., '{lastName}') and contains(., '{middleName}')]";

                var userNode1 = xmlDocument1.SelectSingleNode(xpath);

                if (userNode1 != null)
                {
                    var obj = JObject.Parse(userNode1.InnerText);

                    var email = (string)obj["email"];
                    var base64Image = (string)obj["pict_url"];
                    var fileName = userFromExcel.Name.Replace(" ", "");
                    var imagePath = ImageUtilities.SaveBase64Image(base64Image, fileName, userImgDownloadPath);

                    userFromExcel.ImagePath = imagePath;
                    userFromExcel.Email = email;
                }

                var userNode2 = xmlDocument2.SelectSingleNode(xpath);

                if (userNode1 != null && userNode2 != null)
                {
                    var obj = JObject.Parse(userNode2.InnerText);

                    var login = (string)obj["login"];
                    var password = (string)obj["password"];

                    userFromExcel.Login = login;
                    userFromExcel.Password = password;
                }
            }
        }

        private async Task UpdateOrCreateUsersInDatabase(List<User> usersFromExcel)
        {
            var Date = TermManager.GetDate();

            using (var uow = new UnitOfWork(new ApplicationDbContext(_options)))
            {
                foreach (var userFromExcel in usersFromExcel)
                {
                    try
                    {
                        var existingUser = await uow.Users.GetAsync(x => x.Name == userFromExcel.Name, includeProperties: "Columns.Cards");

                        if (existingUser != null)
                        {
                            existingUser.Position = userFromExcel.Position;
                            existingUser.SspName = userFromExcel.SspName;
                            existingUser.SupervisorName = userFromExcel.SupervisorName;
                            existingUser.Login = userFromExcel.Login;
                            existingUser.Password = userFromExcel.Password;
                            existingUser.ImagePath = userFromExcel.ImagePath;
                            existingUser.Email = userFromExcel.Email;

                            uow.Users.Update(existingUser);
                        }
                        else
                        {
                            var columns = new List<Column>
                                    {
                                        new Column { Title = "Составление SMART-задач работником", Number = 1 },
                                        new Column { Title = "Согласование SMART-задач руководителем", Number = 2 },
                                        new Column { Title = "SMART-задачи к исполнению", Number = 3 },
                                        new Column { Title = "Самооценка работника", Number = 4 },
                                        new Column { Title = "Оценка непосредственного руководителя", Number = 5 },
                                    };

                            userFromExcel.Columns.AddRange(columns);

                            await uow.Users.AddAsync(userFromExcel);
                        }

                        await uow.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Произошла ошибка во время работы с пользователем {userFromExcel.Name} : {ex.Message}. Откат всех изменений.");
                        await uow.RollbackAsync();
                    }
                }
            }
        }

        private async Task CheckForNotifications(List<User> usersFromExcel)
        {
            var Date = TermManager.GetDate();

            using (var uow = new UnitOfWork(new ApplicationDbContext(_options)))
            {
                foreach (var userFromExcel in usersFromExcel)
                {
                    try
                    {
                        var isNeedEmployeeMail = false;
                        var isNeedSupervisorMail = false;
                        var isLastWorkDayOfMonth = false;

                        var mails = await uow.Mails.GetAllAsync();

                        // Проверяем необходимость оповещения на почту исходя из роли
                        if (userFromExcel.Role == Roles.Employee || userFromExcel.Role == Roles.Combined || userFromExcel.Role == Roles.FullAccessSupervisor)
                        {
                            isNeedEmployeeMail = true;
                        }
                        else if (userFromExcel.Role == Roles.Supervisor || userFromExcel.Role == Roles.Combined || userFromExcel.Role == Roles.FullAccessSupervisor)
                        {
                            isNeedSupervisorMail = true;
                        }

                        // Проверяем является ли день последним рабочим днем в месяце
                        var lastDayOfMonth = new DateTime(Date.Year, Date.Month, DateTime.DaysInMonth(Date.Year, Date.Month));

                        if (lastDayOfMonth.DayOfWeek != DayOfWeek.Saturday && lastDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
                        {
                            isLastWorkDayOfMonth = true;
                        }

                        // Оповещение на почту исходя из дня и роли
                        if (Date.Day == 25 && isNeedEmployeeMail)
                        {
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Уведомление", mails.where(x => x.Code == NextQuarterTaskAssignment), userFromExcel.Name));
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Уведомление", mails.where(x => x.Code == EmployeeQuarterlySmartTaskSummary), userFromExcel.Name));
                        }
                        else if (isLastWorkDayOfMonth && isNeedEmployeeMail)
                        {
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Напоминание", mails.where(x => x.Code == NextQuarterTaskAssignment), userFromExcel.Name));
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Напоминание", mails.where(x => x.Code == EmployeeQuarterlySmartTaskSummary), userFromExcel.Name));
                        }
                        else if (Date.Day == 1 && isNeedSupervisorMail)
                        {
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Уведомление", mails.where(x => x.Code == SupervisorQuarterlySmartTaskSummary), userFromExcel.Name));
                            //await _emailSender.SendEmailAsync(new Message(new string[] { userFromExcel.Email }, "Уведомление", mails.where(x => x.Code == SupervisorQuarterlySmartTaskSummary), userFromExcel.Name));
                        }

                        _logger.Info($"Уведомления для пользователя {userFromExcel.Name} успешно отправлено");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Произошла ошибка во время работы с уведомлением пользователя {userFromExcel.Name} : {ex.Message}");
                    }
                }
            }
        }

        private async Task BlockNonActiveUsers(List<User> usersFromExcel)
        {
            using (var uow = new UnitOfWork(new ApplicationDbContext(_options)))
            {
                var allDbUsers = await uow.Users.GetAllAsync();

                foreach (var dbUser in allDbUsers)
                {
                    if (!usersFromExcel.Any(x => x.Name == dbUser.Name))
                    {
                        var nonActiveUser = await uow.Users.GetAsync(x => x.Id == dbUser.Id);
                        nonActiveUser.IsBlocked = true;

                        await uow.CommitAsync();
                    }
                }
            }
        }
    }
}
