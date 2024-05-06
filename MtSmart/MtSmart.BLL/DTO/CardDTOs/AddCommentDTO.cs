namespace MtSmart.BLL.DTO.CardDTOs
{
    public class AddCommentDTO
    {
        public string Comment { get; set; }
        public int CardId { get; set; }

        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
