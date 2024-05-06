namespace MtSmart.BLL.DTO.CardDTOs
{
    public class SetEmployeeAssessmentDTO
    {
        public int CardId { get; set; }
        public int UserId { get; set; }
        public int EmployeeQualityAssessment { get; set; }
        public int EmployeeTermAssessment { get; set; }
        public int HoursOfWork { get; set; }
        public string EmployeeComment { get; set; }

        public string UpdaterName { get; set; }
        public string UpdaterImagePath { get; set; }
    }
}
