using System.ComponentModel.DataAnnotations;

namespace InmobiliariaTPI.Models
{
    public class Propietario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 20 caracteres")]
        [Display(Name = "DNI")]
        public string? Dni { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}