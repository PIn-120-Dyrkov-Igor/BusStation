using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class StopList
    {
        public int Id { get; set; }

        [Display(Name = "Название остановки")]
        public int? StopId { get; set; }
        public Stop? Stop { get; set; }//Где именно может быть пустым

        [Display(Name = "Название маршрута")]
        public int? RouteId { get; set; }
        public Route? Route { get; set; }
    }
}
