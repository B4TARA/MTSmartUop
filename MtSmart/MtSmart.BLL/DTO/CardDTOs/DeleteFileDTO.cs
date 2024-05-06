namespace MtSmart.BLL.DTO.CardDTOs
{
    public class DeleteFileDTO
    {
        public int FileId { get; set; }
        public int CardId { get; set; }

        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
