using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MtSmart.DAL;
using MtSmart.Import;
using NLog;

internal class Program
{
    private static void Main(string[] args)
    {
        DateTime debugToday = DateTime.Today;
     
        var _config = new ConfigurationBuilder().AddJsonFile("D:\\MTSmart\\Configuration\\appsettings.json", optional: false, reloadOnChange: true).Build();

        var _usersPath = _config["FilePaths:UsersPath"];
        var _colsArrayPath = _config["FilePaths:ColsArrayPath"];
        var _usersPassContainerPath = _config["FilePaths:UsersPassContainerPath"];
        var _userImgPath = _config["FilePaths:UserImgDownloadPath"];
        var _emailIconPath = _config["FilePaths:EmailIconPath"];

        var _nlogConfigPath = _config["FilePaths:NLogConfigPath"];
        LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(_nlogConfigPath);
        var _logger = LogManager.GetCurrentClassLogger();

        var _dbConnectionString = _config["ConnectionStrings:WebApiDatabase"];
        var _optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var _options = _optionsBuilder.UseNpgsql(_dbConnectionString).Options;

        var importer = new ExportAndImport(_options);
        importer.TransferUsersFromExcelToDatabase(_usersPath, _colsArrayPath, _usersPassContainerPath, _userImgPath).Wait();
    }
}