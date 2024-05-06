namespace MtSmart.BLL.DTO.CardDTOs
{
    public class SetSupervisorAssessmentDTO
    {
        public int CardId { get; set; }
        public DateOnly FactTerm { get; set; }

        public int SupervisorQualityAssessment { get; set; }
        public int SupervisorTermAssessment { get; set; }
        public string SupervisorComment { get; set; }

        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
