using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Productos
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La descripción del producto es obligatoria.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La descripción debe tener entre 3 y 100 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, 9999999.99, ErrorMessage = "El precio debe ser mayor a $0.")]
        public decimal PreUnitario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de tipo de producto debe ser válido.")]
        public int IdTipoProducto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tamaño de producto.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de tamaño de producto debe ser válido.")]
        public int IdTamañoProducto { get; set; }

        public bool Activo { get; set; } = true;
    }
}
