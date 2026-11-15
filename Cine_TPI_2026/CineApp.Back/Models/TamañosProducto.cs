using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class TamañosProducto
{
    public int IdTamañoProducto { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
