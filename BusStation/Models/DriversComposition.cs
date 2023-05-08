using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class DriversComposition
    {
        public int Id { get; set; }

        
        public int Driver1Id { get; set; }
        [Display(Name = "Первый водитель")]
        public Driver? Driver1 { get; set; }

        
        public int? Driver2Id { get; set; }
        [Display(Name = "Второй водитель")]
        public Driver? Driver2 { get; set; }
    }
}
