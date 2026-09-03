using System.ComponentModel.DataAnnotations;

namespace InmobiliariaTPI.Models
{
    public class Inmueble
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "El cupo máximo es obligatorio")]
        [Range(1, 100, ErrorMessage = "El cupo máximo debe ser entre 1 y 100")]
        public int CupoMaximo { get; set; }

        public string? Coordenadas { get; set; }

        [Required(ErrorMessage = "El precio por día es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioPorDia { get; set; }

        public string? ImagenPortada { get; set; }

        public bool Disponible { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de reserva debe ser entre 0 y 100")]
        public int PorcentajeReserva { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "El propietario es obligatorio")]
        public int IdPropietario { get; set; }

        [Required(ErrorMessage = "El tipo de inmueble es obligatorio")]
        public int IdTipoInmueble { get; set; }
    }
}