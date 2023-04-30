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
        [Range(0001, 9999, ErrorMessage = "Серия паспорта должен содержать 6 цифр")]
        public int PassportSeries { get; set; }

        [Display(Name = "Номер паспорта")]
        [Range(000001, 999999, ErrorMessage = "Номер паспорта должен содержать 6 цифр")]
        public int PassportNumber { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBird { get; set; }
        //Сделать невидимым
        public string? Email { get; set; }
    }
}
