namespace MtSmart.WEB.Models.RequestModels
{
    public class SetSupervisorAssessmentRequestModel
    {
        public int CardId { get; set; }
        public string FactTerm { get; set; }
        public int SupervisorQualityAssessment { get; set; }
        public int SupervisorTermAssessment { get; set; }
        public string SupervisorComment { get; set; }

        public int UserId { get; set; }
    }
}
