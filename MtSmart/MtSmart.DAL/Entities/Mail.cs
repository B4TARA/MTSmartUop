using MtSmart.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Code is required.")]
        public MailCodes Code { get; set; }

        [Required(ErrorMessage = "The Value Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Value Body is required.")]
        public string Body { get; set; }

        
        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
