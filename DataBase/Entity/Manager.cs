using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entity
{
    public class Manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Введите логин учетной страницы")]
        [StringLength(20, MinimumLength = 20, ErrorMessage = "Длина 20 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(25, MinimumLength = 8, ErrorMessage = "Минимальная длина - 8 символов, максимальная - 25 символов")]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
    }
}
