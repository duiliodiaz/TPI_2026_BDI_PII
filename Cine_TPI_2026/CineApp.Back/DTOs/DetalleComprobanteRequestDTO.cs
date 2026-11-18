using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs
{
    public class DetalleComprobanteRequestDTO
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido.")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int CantidadProductos { get; set; }
    }

    public class DetalleComprobanteResponseDTO
    {
        public int IdDetalle { get; set; }

        public int IdProducto { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;

        public int CantidadProductos { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => CantidadProductos * PrecioUnitario;
    }
}
