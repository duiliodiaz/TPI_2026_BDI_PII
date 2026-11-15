using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class DetallesReservaEntradum
{
    public int IdDetalleReservas { get; set; }

    public int IdReserva { get; set; }

    public int IdEntrada { get; set; }

    public virtual Entrada IdEntradaNavigation { get; set; } = null!;

    public virtual Reserva IdReservaNavigation { get; set; } = null!;
}
