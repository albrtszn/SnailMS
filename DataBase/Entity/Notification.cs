using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entity
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Введите сообщение")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Минимальная длина - 5 символов, максимальная - 50 символов")]
        public string Message { get; set; }

        public User User { get; set; }  
    }
}
