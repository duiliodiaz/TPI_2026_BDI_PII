using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public int IdCliente { get; set; }

    public DateTime FechaReserva { get; set; }

    public int IdEstado { get; set; }

    public virtual ICollection<DetallesReservaEntradum> DetallesReservaEntrada { get; set; } = new List<DetallesReservaEntradum>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual EstadosReserva IdEstadoNavigation { get; set; } = null!;
}
