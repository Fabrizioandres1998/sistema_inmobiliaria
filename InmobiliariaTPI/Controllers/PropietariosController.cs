using Microsoft.AspNetCore.Mvc;
using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Controllers;

public class PropietariosController : Controller
{
    public IActionResult Index()
    {
        var propietarios = new List<Propietario>
        {
            new Propietario { Id = 1, Nombre = "Juan" },
            new Propietario { Id = 2, Nombre = "María" }
        };

        return View(propietarios);
    }
}