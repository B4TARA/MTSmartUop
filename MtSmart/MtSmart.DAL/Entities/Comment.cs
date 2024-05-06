using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The Value CommenterName is required.")]
        public string CommenterName { get; set; }

        [Required(ErrorMessage = "The Value CommenterImagePath is required.")]
        public string CommenterImagePath { get; set; }


        public int CardId { get; set; }
        public Card Card { get; set; }


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
