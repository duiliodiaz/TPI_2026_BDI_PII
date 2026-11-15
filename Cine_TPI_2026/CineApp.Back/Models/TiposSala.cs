using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class TiposSala
{
    public int IdTipoSala { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal PrecioEntrada { get; set; }

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}
