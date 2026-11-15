using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Funcione
{
    public int IdFuncion { get; set; }

    public DateTime Fecha { get; set; }

    public int IdPelicula { get; set; }

    public int IdSala { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
