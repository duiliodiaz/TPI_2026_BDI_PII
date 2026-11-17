using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Comprobantes
{
    public class ComprobanteRequestDTO
    {
        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public int IdFormaPago { get; set; }

        public List<DetalleComprobanteRequestDTO> Detalles { get; set; } = new();
    }
}
