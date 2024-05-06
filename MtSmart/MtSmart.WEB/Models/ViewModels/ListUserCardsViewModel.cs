namespace MtSmart.WEB.Models.ViewModels
{
    public class ListUserCardsViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SspName { get; set; }
        public string UserImagePath { get; set; }
        public List<Column> Columns { get; set; } = new();

        public class Column
        {
            public int ColumnId { get; set; }
            public int ColumnNumber { get; set; }
            public string ColumnTitle { get; set; }
            public List<Card> Cards { get; set; } = new();
        }

        public class Card
        {
            public int CardId { get; set; }
            public string CardName { get; set; }
            public string CardRequirement { get; set; }
            public DateOnly CardTerm { get; set; }
            public DateOnly? FactTerm { get; set; }
            public int? SupervisorTermAssessment { get; set; }
            public int CountOfComments { get; set; }
            public int CountOfFiles { get; set; }
        }
    }
}
