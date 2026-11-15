using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Butaca
{
    public int IdButaca { get; set; }

    public string Fila { get; set; } = null!;

    public int Columna { get; set; }

    public int IdSala { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
