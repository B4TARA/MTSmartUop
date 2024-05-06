namespace MtSmart.WEB.Models.RequestModels
{
    public class AddCommentRequestModel
    {
        public string Comment { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
    }
}
