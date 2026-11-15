using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Directore
{
    public int IdDirector { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime? FecNacimiento { get; set; }

    public string? Nacionalidad { get; set; }

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
