using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Comprobante
{
    public int IdComprobante { get; set; }

    public int IdCliente { get; set; }

    public DateTime Fecha { get; set; }

    public int IdFormaPago { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<DetallesComprobanteProducto> DetallesComprobanteProductos { get; set; } = new List<DetallesComprobanteProducto>();

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;
}
