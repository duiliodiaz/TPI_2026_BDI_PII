using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class DetallesComprobanteProducto
{
    public int IdDetalle { get; set; }

    public int IdComprobante { get; set; }

    public int IdProducto { get; set; }

    public int CantidadProductos { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Comprobante IdComprobanteNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
