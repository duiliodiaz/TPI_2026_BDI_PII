using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Pelicula
{
    public int IdPelicula { get; set; }

    public string Titulo { get; set; } = null!;

    public int IdGenero { get; set; }

    public int Duracion { get; set; }

    public DateTime? FechaEstreno { get; set; }

    public int IdClasificacion { get; set; }

    public string? Sinopsis { get; set; }

    public int IdDirector { get; set; }

    public virtual ICollection<DetallesPeliculaActor> DetallesPeliculaActors { get; set; } = new List<DetallesPeliculaActor>();

    public virtual ICollection<Funcione> Funciones { get; set; } = new List<Funcione>();

    public virtual Clasificacione IdClasificacionNavigation { get; set; } = null!;

    public virtual Directore IdDirectorNavigation { get; set; } = null!;

    public virtual Genero IdGeneroNavigation { get; set; } = null!;
}
