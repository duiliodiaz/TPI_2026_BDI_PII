using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs.Comprobantes
{
    public class ComprobanteResponseDTO
    {
        public int IdComprobante { get; set; }

        public int IdCliente { get; set; }

        public string ClienteNombreCompleto { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public int IdFormaPago { get; set; }

        public string FormaPagoNombre { get; set; } = string.Empty;

        public decimal Total => Detalles.Sum(d => d.Subtotal);

        public List<DetalleComprobanteResponseDTO> Detalles { get; set; } = new();
    }
}
