using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Comprobantes
{
    public class DetalleComprobanteRequestDTO
    {
        public int IdProducto { get; set; }
        public int CantidadProductos { get; set; }
    }
}
