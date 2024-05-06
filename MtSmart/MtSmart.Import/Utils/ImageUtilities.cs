using NLog;

namespace MtSmart.Import.Utils
{
    public static class ImageUtilities
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static string SaveBase64Image(string base64String, string fileName, string userImgDownloadPath)
        {
            try
            {
                var file = Path.Combine(userImgDownloadPath, $"{fileName}.jpg");

                byte[] bytes = Convert.FromBase64String(base64String);

                using (FileStream fileStream = new FileStream(file, FileMode.Create))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Flush();
                }

                return file;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

                return "../image/default_profile_icon.svg";
            }
        }
    }
}
