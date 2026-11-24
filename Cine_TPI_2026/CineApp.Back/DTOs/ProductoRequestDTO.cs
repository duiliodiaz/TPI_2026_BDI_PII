using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs
{
    public class ProductoRequestDTO
    {
        [Required(ErrorMessage = "La descripción del producto es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s]+$", ErrorMessage = "La descripción solo puede contener letras, números y espacios.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        public decimal PreUnitario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de tipo de producto debe ser válido.")]
        public int IdTipoProducto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tamaño de producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de tamaño de producto debe ser válido.")]
        public int IdTamañoProducto { get; set; }

        // null = no se modifica el estado (solo se usa en el PUT)
        public bool? Activo { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 5000, ErrorMessage = "El stock debe estar entre 0 y 5000 unidades.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, 5000, ErrorMessage = "El stock debe estar entre 0 y 5000 unidades.")]
        public int StockMinimo { get; set; }
    }

    public class ProductoResponseDTO
    {
        public int IdProducto { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal PreUnitario { get; set; }

        public int IdTipoProducto { get; set; }

        public int IdTamañoProducto { get; set; }

        public string TipoProductoNombre { get; set; }

        public string TamañoProductoNombre { get; set; }

        public bool Activo { get; set; } = true;

        public int Stock { get; set; }

        public int StockMinimo { get; set; }
    }
}
