namespace InmobiliariaTPI.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Dni { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}