using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class VwResumenMensual
{
    public int? Año { get; set; }

    public int? Mes { get; set; }

    public string Película { get; set; } = null!;

    public string Género { get; set; } = null!;

    public string TipoDeSala { get; set; } = null!;

    public int? CantidadTotalDeEntradasVendidas { get; set; }

    public decimal? RecaudaciónTotal { get; set; }
}
