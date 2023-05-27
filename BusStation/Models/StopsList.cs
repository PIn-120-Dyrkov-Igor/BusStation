using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class StopsList
    {
        public int Id { get; set; }
     
        public int? RouteId { get; set; }
        [Display(Name = "Название маршрута")]
        public Route? Route { get; set; }

        public int? StopId { get; set; }
        [Display(Name = "Название остановки")]
        public Stop? Stop { get; set; }

        
    }
}
