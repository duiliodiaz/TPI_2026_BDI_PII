using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.DTOs
{
    public class ComprobanteRequestDTO
    {
        public int IdComprobante { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido.")]
        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La forma de pago es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una forma de pago válida.")]
        public int IdFormaPago { get; set; }

        [Required(ErrorMessage = "El comprobante debe incluir detalles.")]
        [MinLength(1, ErrorMessage = "El comprobante debe tener al menos un producto en el detalle.")]
        public List<DetalleComprobanteRequestDTO> Detalles { get; set; } = new();
    }

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
