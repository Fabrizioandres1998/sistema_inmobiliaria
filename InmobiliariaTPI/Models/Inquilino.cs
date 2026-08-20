using System.ComponentModel.DataAnnotations;

namespace InmobiliariaTPI.Models
{
    public class Inquilino
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto { get; set; }

        [Display(Name = "DNI")]
        public string? Dni { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}