using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Value Requirement is required.")]
        public string Requirement { get; set; }

        [Required(ErrorMessage = "The Value StartTerm is required.")]
        public DateOnly StartTerm { get; set; }

        [Required(ErrorMessage = "The Value Term is required.")]
        public DateOnly Term { get; set; }
        public DateOnly? FactTerm { get; set; }


        public int? EmployeeQualityAssessment { get; set; }
        public int? EmployeeTermAssessment { get; set; }
        public string? EmployeeComment { get; set; }

        public int? SupervisorQualityAssessment { get; set; }
        public int? SupervisorTermAssessment { get; set; }
        public string? SupervisorComment { get; set; }

        public int? HoursOfWork { get; set; }

        public bool IsRelevant { get; set; }
        public bool IsDeleted { get; set; }
        public bool ReadyToReport { get; set; }


        public int CountOfComments { get; set; }
        public int CountOfFiles { get; set; }

        public int ColumnId { get; set; }
        public Column Column { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }


        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<File> Files { get; set; } = new List<File>();
        public List<Update> Updates { get; set; } = new List<Update>();


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
