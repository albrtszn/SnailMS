using System.ComponentModel.DataAnnotations;

namespace SnailMS.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Введите сообщение")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Минимальная длина - 5 символов, максимальная - 50 символов")]
        public string Message { get; set; }
    }
}
