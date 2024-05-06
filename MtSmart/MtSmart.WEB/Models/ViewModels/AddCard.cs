namespace MtSmart.WEB.Models.ViewModels
{
    public class AddCard
    {
        public string CardName { get; set; }

        public string CardRequirement { get; set; }

        public DateOnly CardTerm { get; set; }

        public DateOnly Min { get; set; }


        public int UserId { get; set; }
    }
}
