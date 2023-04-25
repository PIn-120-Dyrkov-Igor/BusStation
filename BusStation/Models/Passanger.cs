using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BusStation.Models
{
    public class Passanger
    {
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(20, MinimumLength = 2)]
        public string Surname { get; set; }

        [Display(Name = "Имя")]
        [StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(20, MinimumLength = 2)]
        public string Patronymic { get; set; }

        [Display(Name = "Серия паспорта")]
        [MinLength(4)]
        [MaxLength(4)]
        public int PassportSeries { get; set; }

        [Display(Name = "Номер паспорта")]
        [MinLength(6)]
        [MaxLength(6)]
        public int PassportNumber { get; set; }

        public DateTime DateOfBird { get; set; }
        //Сделать невидимым
        public string? Email { get; set; }
    }
}
