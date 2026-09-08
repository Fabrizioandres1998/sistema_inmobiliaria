using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected void SetSuccessMessage(string message)
        {
            TempData["Mensaje"] = message;
            TempData["Tipo"] = "success";
        }

        protected void SetErrorMessage(string message)
        {
            TempData["Mensaje"] = message;
            TempData["Tipo"] = "danger";
        }

        protected void SetWarningMessage(string message)
        {
            TempData["Mensaje"] = message;
            TempData["Tipo"] = "warning";
        }

        protected void AddModelErrors(Exception ex)
        {
            _logger.LogError(ex, "Error en operación");
            ModelState.AddModelError("", ex.Message);
            SetErrorMessage(ex.Message);
        }
    }
}