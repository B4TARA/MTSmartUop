namespace MtSmart.WEB.Models.RequestModels
{
    public class UploadFileRequestModel
    {
        public IFormFile[] FilesToUpload { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
    }
}
