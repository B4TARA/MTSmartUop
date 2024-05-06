namespace MtSmart.WEB.Models.ViewModels
{
    public class GetReportViewModel
    {
        public string EmployeeSspName { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeePosition { get; set; } = string.Empty;
        public string SupervisorName { get; set; } = string.Empty;

        public string CardName { get; set; } = string.Empty;
        public string CardRequirement { get; set; } = string.Empty;
        public string CardStartTerm { get; set; } = string.Empty;
        public string CardFactTerm { get; set; } = string.Empty;

        public string EmployeeQualityAssessmentText { get; set; } = string.Empty;
        public string EmployeeTermAssessmentText { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public int HoursOfWork { get; set; }

        public string SupervisorQualityAssessmentText { get; set; } = string.Empty;
        public string SupervisorTermAssessmentText { get; set; } = string.Empty;
        public string SupervisorComment { get; set; } = string.Empty;

        public string SupervisorQualityAssessmentValue { get; set; } = string.Empty;
        public string SupervisorTermAssessmentValue { get; set; } = string.Empty;
    }
}
