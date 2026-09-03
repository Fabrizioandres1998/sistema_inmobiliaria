using InmobiliariaTPI.Models;
public class DashboardViewModel
{
    public int TotalPropietarios { get; set; }
    public int TotalInmuebles { get; set; }
    public int ReservasActivas { get; set; }
    public List<Inquilino>? UltimosInquilinos { get; set; }
}