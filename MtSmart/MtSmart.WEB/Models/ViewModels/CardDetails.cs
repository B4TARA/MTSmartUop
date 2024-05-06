using MtSmart.DAL.Entities;
using MtSmart.DAL.Enums;
using File = MtSmart.DAL.Entities.File;

namespace MtSmart.WEB.Models.ViewModels
{
    public class CardDetails
    {
        public int CardId { get; set; }
        public string CardName { get; set; } = string.Empty;
        public string CardRequirement { get; set; } = string.Empty;
        public DateOnly CardTerm { get; set; }

        public DateOnly? FactTerm { get; set; }
        public int? HoursOfWork { get; set; }
        public int? EmployeeQualityAssessment { get; set; }
        public int? EmployeeTermAssessment { get; set; }
        public string? EmployeeComment { get; set; }
        public int? SupervisorQualityAssessment { get; set; }
        public int? SupervisorTermAssessment { get; set; }
        public string? SupervisorComment { get; set; }

        public int Column { get; set; } = 1;
        public int UserId { get; set; }
        public int ColumnId { get; set; }
        public int ColumnNumber { get; set; }

        public bool IsRelevant { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsThisQuarterCard { get; set; }

        public List<AssessmentQualityResult> assessmentQualityResults = new();
        public List<AssessmentTermResult> assessmentTermResults = new();

        public List<Comment> Comments = new();
        public List<File> Files = new();
        public List<Update> Updates = new();

        public int CurrentUserId { get; set; }
        public bool IsFullAccessSupervisor { get; set; }

        public bool IsNeedSupervisorAssessment { get; set; }
        public bool IsNeedEmployeeAssessment { get; set; }
    }
}
