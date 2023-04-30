using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Display(Name = "Номер маршрута")]
        [Range(1, 999, ErrorMessage = "Номер маршрута должен содержать от 1 до 3 цифр")]
        public int RouteNumber { get; set; }

        [Display(Name = "Город отправления")]
        public string DepertureCity { get; set; }

        [Display(Name = "Город прибытия")]
        public string ArrivalCity { get; set; }

        [Display(Name = "Время в пути")]
        [DataType(DataType.Time)]
        public DateTime TravelTime { get; set; }
    }
}
