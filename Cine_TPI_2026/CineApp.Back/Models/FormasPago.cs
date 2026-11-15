using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class FormasPago
{
    public int IdFormaPago { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();
}
