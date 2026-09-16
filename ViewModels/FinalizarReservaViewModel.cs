using System.ComponentModel.DataAnnotations;

namespace InmobiliariaTPI.ViewModels
{
    public class FinalizarReservaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de terminación es obligatoria")]
        [Display(Name = "Fecha de Terminación")]
        public DateTime FechaTerminacion { get; set; }

        [Display(Name = "Multa Calculada")]
        public decimal MultaCalculada { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal MontoPorDia { get; set; }
        public string? InmuebleDireccion { get; set; }
        public string? InquilinoNombre { get; set; }
        public int IdUsuarioTerminacion { get; set; }
    }
}