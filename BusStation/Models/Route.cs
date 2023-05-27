using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Display(Name = "Номер маршрута")]
        [RegularExpression(@"^(0[0-9]{2}|[1-9][0-9]{2})$", ErrorMessage = "Номер маршрута должен содержать 3 цифры(Диапазон: 001-999)")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public int RouteNumber { get; set; }

        [Display(Name = "Город отправления")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string DepertureCity { get; set; }

        [Display(Name = "Город прибытия")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string ArrivalCity { get; set; }

        [Display(Name = "Время в пути")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        [DataType(DataType.Time)]
        public DateTime TravelTime { get; set; }
    }
}
