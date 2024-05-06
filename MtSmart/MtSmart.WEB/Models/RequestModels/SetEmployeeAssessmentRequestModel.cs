namespace MtSmart.WEB.Models.RequestModels
{
    public class SetEmployeeAssessmentRequestModel
    {
        public int CardId { get; set; }
        public int EmployeeQualityAssessment { get; set; }
        public int EmployeeTermAssessment { get; set; }
        public int HoursOfWork { get; set; }
        public string EmployeeComment { get; set; }

        public int UserId { get; set; }
    }
}
