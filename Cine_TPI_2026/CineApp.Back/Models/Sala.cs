using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Sala
{
    public int IdSala { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdTipoSala { get; set; }

    public int Capacidad { get; set; }

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();

    public virtual ICollection<Funcione> Funciones { get; set; } = new List<Funcione>();

    public virtual TiposSala IdTipoSalaNavigation { get; set; } = null!;
}
