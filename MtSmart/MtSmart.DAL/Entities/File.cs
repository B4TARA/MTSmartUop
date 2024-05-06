using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class File
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Value Path is required.")]
        public string Path { get; set; }

        [Required(ErrorMessage = "The Value Size is required.")]
        public long Size { get; set; }

        [Required(ErrorMessage = "The Value Type is required.")]
        public string Type { get; set; }


        public int CardId { get; set; }
        public Card Card { get; set; }


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
