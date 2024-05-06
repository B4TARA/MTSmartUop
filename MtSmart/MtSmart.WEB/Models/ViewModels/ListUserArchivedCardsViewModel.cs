namespace MtSmart.WEB.Models.ViewModels
{
    public class ListUserArchivedCardsViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SspName { get; set; }
        public string UserImagePath { get; set; }
        public List<Card> ArchivedCards { get; set; } = new();

        public class Card
        {
            public int CardId { get; set; }
            public string CardName { get; set; }
            public string CardRequirement { get; set; }
            public DateOnly CardTerm { get; set; }
            public DateOnly FactTerm { get; set; }
            public int? HoursOfWork { get; set; }
            public string? EmployeeQualityAssessment { get; set; }
            public string? EmployeeTermAssessment { get; set; }
            public string? SupervisorQualityAssessment { get; set; }
            public string? SupervisorTermAssessment { get; set; }
            public int CountOfComments { get; set; }
            public int CountOfFiles { get; set; }
        }
    }
}
