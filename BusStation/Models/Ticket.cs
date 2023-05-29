using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Display(Name = "Номер билета")]
        public int? TicketNumber { get; set; }

        [Display(Name = "Дата и время продажи")]
        public DateTime? DateSale { get; set; }

        [Display(Name = "Номер маршрута")]
        public int? RouteId { get; set; }
        public Route? Route { get; set; }

        [Display(Name = "Номер рейса")]
        public int? TripId { get; set; }
        public Trip? Trip { get; set; }

        [Display(Name = "Номер пассажира в системе")]
        public int? PassangerId { get; set; }
        public Passanger? Passanger { get; set; }

        [Display(Name = "Пасадочное место")]
        public int? SeatNumber { get; set; }
    }
}
