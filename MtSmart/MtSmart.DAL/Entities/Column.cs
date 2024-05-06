using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class Column
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Value Number is required.")]
        public int Number { get; set; }

        [Required(ErrorMessage = "The Value Title is required.")]
        public string Title { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }


        public List<Card> Cards { get; set; } = new();


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
