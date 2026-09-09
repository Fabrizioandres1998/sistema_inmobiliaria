using InmobiliariaTPI.Models;
using InmobiliariaTPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InmobiliariaTPI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUsuarioService _usuarioService;

        public AuthController(IUsuarioService usuarioService, ILogger<AuthController> logger)
            : base(logger)
        {
            _usuarioService = usuarioService;
        }

        // GET: Auth/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            _logger.LogInformation("Mostrando formulario de login");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            _logger.LogInformation("Intento de login para: {Email}", email);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email y contraseña son obligatorios");
                return View();
            }

            var usuario = await _usuarioService.GetByEmailAsync(email);

            if (usuario == null)
            {
                _logger.LogWarning("Usuario no encontrado: {Email}", email);
                ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View();
            }

            // verificar contraseña
            if (usuario.Password != password)
            {
                _logger.LogWarning("Contraseña incorrecta para: {Email}", email);
                ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View();
            }

            // crear claims para la cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? string.Empty),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("Usuario {Email} logueado correctamente", email);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: Auth/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Cerrando sesion");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Auth/AccessDenied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Acceso denegado");
            return View();
        }
    }
}