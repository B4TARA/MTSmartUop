using System.ComponentModel.DataAnnotations;

namespace MtSmart.WEB.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Логин")]
        [MaxLength(20, ErrorMessage = "Логин должен иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Логин должен иметь длину больше 3 символов")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [MaxLength(20, ErrorMessage = "Пароль должен иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Пароль должен иметь длину больше 3 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail адрес")]
        public string? Email { get; set; }
    }
}
