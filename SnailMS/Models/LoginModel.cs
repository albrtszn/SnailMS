using System.ComponentModel.DataAnnotations;

namespace SnailMS.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите номер телефона")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Длина 13 символов -> +375XXYYYZZCC")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Минимальная длина - 8 символов, максимальная - 25 символов")]
        public string Password { get; set; }
    }
}
