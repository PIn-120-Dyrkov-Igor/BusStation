using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Driver
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

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateOfBird { get; set; }

        [Display(Name = "Номер медицинской карты")]
        [Range(000001, 999999, ErrorMessage = "Номер медицинской карты должен содержать 6 цифр")]
        public int MedecineCard { get; set; }

        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^89\d{9}$", ErrorMessage = "Номер телефона должен содержать 11 цифр")]
        public int? PhoneNumber { get; set; }

        [Display(Name = "Фотография водителя")]
        public string? Photo { get; set; }
    }
}
