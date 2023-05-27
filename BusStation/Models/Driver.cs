using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Фамилия должена состоять только из букв русского или английского алфавита")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 20 символов.")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string Surname { get; set; }

        [Display(Name = "Имя")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Имя должена состоять только из букв русского или английского алфавита")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 20 символов.")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [RegularExpression(@"^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Отчество должена состоять только из букв русского или английского алфавита")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 20 символов.")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string Patronymic { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Введите значение меньшее или равное текущей дате")]
        public DateTime DateOfBird { get; set; }

        [Display(Name = "Номер медицинской карты")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Номер медицинской карты должен содержать шесть цифр.")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string MedecineCard { get; set; }

        [Display(Name = "Фотография водителя")]
        public string? Photo { get; set; }
    }
}
