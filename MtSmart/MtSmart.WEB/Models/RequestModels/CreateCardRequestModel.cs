namespace MtSmart.WEB.Models.RequestModels
{
    public class CreateCardRequestModel
    {
        public int UserId { get; set; }

        public string CardName { get; set; }

        public string CardRequirement { get; set; }

        public string CardTerm { get; set; }
    }
}
