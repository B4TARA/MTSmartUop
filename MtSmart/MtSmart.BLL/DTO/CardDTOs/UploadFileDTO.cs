using Microsoft.AspNetCore.Http;

namespace MtSmart.BLL.DTO.CardDTOs
{
    public class UploadFileDTO
    {
        public IFormFile[] FilesToUpload { get; set; }
        public int CardId { get; set; }

        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
