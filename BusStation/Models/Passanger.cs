using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Passanger
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

        [Display(Name = "Серия паспорта")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Серия паспорта должена содержать 4 цифры")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string PassportSeries { get; set; }

        [Display(Name = "Номер паспорта")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Номер паспорта должен содержать 6 цифр")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string PassportNumber { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Введите значение меньшее или равное текущей дате")]
        public DateTime DateOfBird { get; set; }
        //Заносим Email по которому был зарагестрирован пассажир
        public string? Email { get; set; }
    }
}
