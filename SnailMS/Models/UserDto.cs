using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SnailMS.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Длина 15 символов (2-min)")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Длина 15 символов (2-min)")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Введите отчество")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Длина 15 символов (5-min)")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Введите номер телефона")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Длина 13 символов -> +375XXYYYZZCC")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Минимальная длина - 8 символов, максимальная - 25 символов")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Введите адрес")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина 50 символов (5-min)")]
        public string Adress { get; set; }
        public DateTime EntryDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string Access { get; set; }
    }
}
