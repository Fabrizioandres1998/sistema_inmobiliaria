// using System.ComponentModel.DataAnnotations;

// namespace InmobiliariaTPI.Models
// {
//     public class Usuario
//     {
//         [Key]
//         [Display(Name = "ID")]
//         public int Id { get; set; }

//         [Required(ErrorMessage = "El email es obligatorio")]
//         [EmailAddress(ErrorMessage = "El email no es válido")]
//         [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres")]
//         [Display(Name = "Email")]
//         public string? Email { get; set; }

//         [Required(ErrorMessage = "La contraseña es obligatoria")]
//         [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
//         [Display(Name = "Contraseña")]
//         public string? Password { get; set; }

//         [Display(Name = "Rol")]
//         public RolUsuario Rol { get; set; }

//         [Required(ErrorMessage = "El nombre completo es obligatorio")]
//         [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
//         [Display(Name = "Nombre completo")]
//         public string? NombreCompleto { get; set; }

//         [Display(Name = "Avatar")]
//         public string? Avatar { get; set; }

//         [Display(Name = "Fecha de creación")]
//         public DateTime FechaCreacion { get; set; }

//         [Display(Name = "Fecha de última modificación")]
//         public DateTime? FechaUltimaModificacion { get; set; }
//     }
// }