using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class TiposDescuento
{
    public int IdTipoDescuento { get; set; }

    public decimal Descripcion { get; set; }

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
