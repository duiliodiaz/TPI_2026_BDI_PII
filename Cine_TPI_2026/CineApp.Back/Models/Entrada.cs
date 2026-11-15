using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Entrada
{
    public int IdEntrada { get; set; }

    public int IdFuncion { get; set; }

    public int IdButaca { get; set; }

    public int Edad { get; set; }

    public decimal PrecioCobrado { get; set; }

    public int? IdComprobante { get; set; }

    public int? IdPromocion { get; set; }

    public virtual ICollection<DetallesReservaEntradum> DetallesReservaEntrada { get; set; } = new List<DetallesReservaEntradum>();

    public virtual Butaca IdButacaNavigation { get; set; } = null!;

    public virtual Comprobante? IdComprobanteNavigation { get; set; }

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;

    public virtual Promocione? IdPromocionNavigation { get; set; }
}
