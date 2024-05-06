using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class Update
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Content is required.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "The Value UpdaterName is required.")]
        public string UpdaterName { get; set; }

        [Required(ErrorMessage = "The Value UpdaterImagePath is required.")]
        public string UpdaterImagePath { get; set; }


        public int CardId { get; set; }
        public Card Card { get; set; }


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
