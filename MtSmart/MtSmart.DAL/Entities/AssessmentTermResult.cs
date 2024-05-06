using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class AssessmentTermResult
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Value is required.")]
        public string Value { get; set; }

        [Required(ErrorMessage = "The Value Text is required.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "The Value Description is required.")]
        public string Description { get; set; }


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
