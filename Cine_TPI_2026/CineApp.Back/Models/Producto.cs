using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal PreUnitario { get; set; }

    public int IdTipoProducto { get; set; }

    public int IdTamañoProducto { get; set; }

    public bool Activo { get; set; } = true;

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public virtual ICollection<DetallesComprobanteProducto> DetallesComprobanteProductos { get; set; } = new List<DetallesComprobanteProducto>();

    public virtual TamañosProducto IdTamañoProductoNavigation { get; set; } = null!;

    public virtual TiposProducto IdTipoProductoNavigation { get; set; } = null!;
}
