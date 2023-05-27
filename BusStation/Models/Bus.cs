using System.ComponentModel.DataAnnotations;

namespace BusStation.Models
{
    public class Bus
    {
        public int Id { get; set; }

        [Display(Name = "Номер автобуса")]
        [RegularExpression(@"^[а-яА-Я]\d{3}[а-яА-Я]{2}\d{2,3}", ErrorMessage = "Введите номер автобуса(Формата: А123БВ45)")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public string BusName { get; set; }

        [Display(Name = "Количество мест")]
        [RegularExpression("^(?:2[04]|[34][048])$", ErrorMessage = "Введите число от 20 до 48, кратное 4.")]
        [Required(ErrorMessage = "Это поле бязательно для заполнения")]
        public int SeatNumber { get; set; }
    }
}
