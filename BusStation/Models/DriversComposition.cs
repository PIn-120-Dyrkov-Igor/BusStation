using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class DriversComposition
    {
        public int Id { get; set; }

        [Display(Name = "Первый водитель")]
        public int Driver1Id { get; set; }
        public Driver Driver1 { get; set; }

        [Display(Name = "Второй водитель")]
        public int? Driver2Id { get; set; }
        public Driver? Driver2 { get; set; }
    }
}
