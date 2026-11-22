using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs
{
    public class ClienteDTO
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(1000000, 99999999, ErrorMessage = "El DNI debe ser un número válido de 7 u 8 dígitos.")]
        public long Dni { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        public string? EMail { get; set; }

        public bool Activo { get; set; } = true;
    }


    public class ClienteRequestDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [Range(1000000, 99999999, ErrorMessage = "El DNI debe ser un número válido de 7 u 8 dígitos.")]
        public long Dni { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El email no puede superar los 100 caracteres.")]
        public string? EMail { get; set; }

        public bool Activo { get; set; } = true;
    }
}
