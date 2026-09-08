using System.ComponentModel.DataAnnotations;

namespace InmobiliariaTPI.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Fecha de fin original")]
        public DateTime? FechaFinOriginal { get; set; }

        [Required(ErrorMessage = "El monto por dia es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
        [DisplayFormat(DataFormatString = "{0:#,##0.##}", ApplyFormatInEditMode = true)]
        [Display(Name = "Monto por día")]
        public decimal MontoPorDia { get; set; }

        public string? Estado { get; set; }  // Activa, Finalizada, Cancelada
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; }
        [Display(Name = "Fecha de terminación")]
        public DateTime? FechaTerminacion { get; set; }

        [Range(0, 100, ErrorMessage = "La multa debe ser entre 0 y 100")]
        [Display(Name = "Multa aplicada")]
        public decimal? MultaAplicada { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio")]
        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        public int IdInmueble { get; set; }

        [Required(ErrorMessage = "El usuario creador es obligatorio")]
        public int IdUsuarioCreador { get; set; }

        public int? IdUsuarioTerminacion { get; set; }

        // Propiedades de navegacion
        public virtual Inquilino? Inquilino { get; set; }
        public virtual Inmueble? Inmueble { get; set; }
        // public virtual Usuario? UsuarioCreador { get; set; }
        // public virtual Usuario? UsuarioTerminacion { get; set; }
    }
}
