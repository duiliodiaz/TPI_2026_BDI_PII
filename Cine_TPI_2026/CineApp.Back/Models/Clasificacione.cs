using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Clasificacione
{
    public int IdClasificacion { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
