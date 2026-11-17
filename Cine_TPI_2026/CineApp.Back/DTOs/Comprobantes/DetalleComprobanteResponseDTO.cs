using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Comprobantes
{
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
