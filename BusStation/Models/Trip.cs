using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Trip
    {
        public int Id { get; set; }

        
        public int? BusId { get; set; }
        [Display(Name = "Номер автобуса")]
        public Bus? Bus { get; set; }

        
        public int? RouteId { get; set; }
        [Display(Name = "Номер маршрута")]
        public Route? Route { get; set; }

        
        public int? DriversCompositionId { get; set; }
        [Display(Name = "Состав водителей")]
        public DriversComposition? DriversComposition { get; set; }

        [Display(Name = "Количество свободных мест")]
        public int FreeSeatCount { get; set; }

        [Display(Name = "Дата рейса")]
        [DataType(DataType.Date)]
        public DateTime TripDate { get; set; }

        [Display(Name = "Время рейса")]
        [DataType(DataType.Time)]
        public DateTime TripTime { get; set; }

        [Display(Name = "Дата прибытия")]
        [DataType(DataType.Date)]
        public DateTime TripDateArrival { get; set; }

        [Display(Name = "Время прибытия")]
        [DataType(DataType.Time)]
        public DateTime TripTimeArrival { get; set; }
    }
}
