using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaTPI.Models
{
    public class Reserva
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Fecha Fin Original")]
        [DataType(DataType.Date)]
        public DateTime? FechaFinOriginal { get; set; }

        [Required(ErrorMessage = "El monto por día es obligatorio")]
        [Display(Name = "Monto por Día")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal MontoPorDia { get; set; }

        [Display(Name = "Monto Total")]
        [DataType(DataType.Currency)]
        public decimal MontoTotal
        {
            get
            {
                var dias = (FechaFin.Date - FechaInicio.Date).Days;
                if (dias <= 0) dias = 1;
                return MontoPorDia * dias;
            }
        }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activa";

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Terminación")]
        [DataType(DataType.Date)]
        public DateTime? FechaTerminacion { get; set; }

        [Display(Name = "Multa Aplicada")]
        [DataType(DataType.Currency)]
        public decimal? MultaAplicada { get; set; }

        // Relaciones
        [Required(ErrorMessage = "El inquilino es obligatorio")]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        [ForeignKey("IdInquilino")]
        public virtual Inquilino? Inquilino { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }

        [ForeignKey("IdInmueble")]
        public virtual Inmueble? Inmueble { get; set; }

        [Display(Name = "Usuario Creador")]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [Display(Name = "Usuario Terminación")]
        public int? IdUsuarioTerminacion { get; set; }

        [ForeignKey("IdUsuarioTerminacion")]
        public virtual Usuario? UsuarioTerminacion { get; set; }
    }
}