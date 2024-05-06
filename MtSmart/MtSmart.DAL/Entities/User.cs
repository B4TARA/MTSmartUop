using MtSmart.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace MtSmart.DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Position { get; set; }
        public string? SspName { get; set; }
        public string? ImagePath { get; set; }
        public string? SupervisorName { get; set; }


        public Roles Role { get; set; }


        public bool IsBlocked { get; set; } = false;


        public List<Column> Columns { get; set; } = new();


        public DateOnly DateOfCreation { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    }
}
