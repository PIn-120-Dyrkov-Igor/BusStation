using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Stop
    {
        public int Id { get; set; }

        [Display(Name = "Название остановки")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string StopName { get; set; }

        [Display(Name = "Город остановки")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string StopCity { get; set; }

        [Display(Name = "Цена")]
        [RegularExpression(@"^(?:[1-9]|[1-9][0-9]{1,2}|999)$", ErrorMessage = "Введите число от 1 до 999")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public int Price { get; set; }

        [Display(Name = "Время до остановки")]
        [DataType(DataType.Time)]
        public DateTime TimeToStop { get; set; }
    }
}
