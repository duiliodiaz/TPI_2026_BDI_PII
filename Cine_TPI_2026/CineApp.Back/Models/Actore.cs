using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Actore
{
    public int IdActor { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime? FecNacimiento { get; set; }

    public string? Nacionalidad { get; set; }

    public virtual ICollection<DetallesPeliculaActor> DetallesPeliculaActors { get; set; } = new List<DetallesPeliculaActor>();
}
