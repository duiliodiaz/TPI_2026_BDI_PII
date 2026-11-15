using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Promocione
{
    public int IdPromocion { get; set; }

    public string Dia { get; set; } = null!;

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public int IdTipoDescuento { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual TiposDescuento IdTipoDescuentoNavigation { get; set; } = null!;
}
