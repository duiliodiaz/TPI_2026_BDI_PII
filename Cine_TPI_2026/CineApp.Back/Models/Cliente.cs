using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public long Dni { get; set; }

    public string? EMail { get; set; }

    public bool Activo { get; set; } = true;

    public virtual ICollection<Comprobante> Comprobantes { get; set; } = new List<Comprobante>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
