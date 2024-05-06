namespace MtSmart.WEB.Models.RequestModels
{
    public class MoveCardRequestModel
    {
        public int CardId { get; set; }

        public string CardName { get; set; }

        public string CardRequirement { get; set; }

        public string CardTerm { get; set; }

        public int UserId { get; set; }
    }
}
