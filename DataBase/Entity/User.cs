using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Введите номер телефона")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Длина 13 символов -> +375XXYYYZZCC")]
        public string Number { get; set; }
        //[Required(ErrorMessage = "Введите пароль")]
        //[StringLength(25, MinimumLength = 8, ErrorMessage = "Минимальная длина - 8 символов, максимальная - 25 символов")]
        public string Password { get; set; }
        public string Adress { get; set; }
        public DateTime EntryDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string Access { get; set; }
        public byte[]? Picture { get; set; }

        public List<Call> Calls { get; set; }
        public List<TempCall> TempCalls { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
