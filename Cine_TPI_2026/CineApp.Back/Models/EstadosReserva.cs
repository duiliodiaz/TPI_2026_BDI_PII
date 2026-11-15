using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class EstadosReserva
{
    public int IdEstadoReserva { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
