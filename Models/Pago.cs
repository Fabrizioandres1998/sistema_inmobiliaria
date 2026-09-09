using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaTPI.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El concepto es obligatorio")]
        [Display(Name = "Concepto")]
        [StringLength(200, ErrorMessage = "El concepto no puede superar los 200 caracteres")]
        public string Concepto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de pago es obligatoria")]
        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El importe es obligatorio")]
        [Display(Name = "Importe")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal Importe { get; set; }

        [Display(Name = "Estado")]
        public byte Estado { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Anulación")]
        public DateTime? FechaAnulacion { get; set; }

        // Relaciones
        [Required(ErrorMessage = "La reserva es obligatoria")]
        [Display(Name = "Reserva")]
        public int IdReserva { get; set; }

        [ForeignKey("IdReserva")]
        public virtual Reserva? Reserva { get; set; }

        [Required(ErrorMessage = "El usuario creador es obligatorio")]
        [Display(Name = "Usuario Creador")]
        public int IdUsuarioCreador { get; set; }

        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [Display(Name = "Usuario Anulación")]
        public int? IdUsuarioAnulacion { get; set; }

        [ForeignKey("IdUsuarioAnulacion")]
        public virtual Usuario? UsuarioAnulacion { get; set; }
    }
}