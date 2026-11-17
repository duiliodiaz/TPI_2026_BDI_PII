using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Productos
{
    public class ProductoResponseDTO
    {
        public int IdProducto { get; set; }

        public string Descripcion { get; set; } = null!;

        public decimal PreUnitario { get; set; }

        public int IdTipoProducto { get; set; }

        public int IdTamañoProducto { get; set; }

        public string TipoProductoNombre { get; set; }

        public string TamañoProductoNombre { get; set; }
    }
}
