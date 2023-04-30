using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Bus
    {
        public int Id { get; set; }

        [Display(Name = "Номер автобуса")]
        [MinLength(3)]
        [MaxLength(12)]
        public string BusName { get; set; }

        [Display(Name = "Количество мест")]
        [RegularExpression(@"^[2468][0-9]*$", ErrorMessage = "Введите четное число, отличное от нуля")]      
        public int SeatNumber { get; set; }
    }
}
