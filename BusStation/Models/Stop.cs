using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Stop
    {
        public int Id { get; set; }

        [Display(Name = "Название остановки")]
        public string StopName { get; set; }

        [Display(Name = "Город остановки")]
        public string StopCity { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Время до остановки")]
        [DataType(DataType.Time)]
        public DateTime TimeToStop { get; set; }
    }
}
