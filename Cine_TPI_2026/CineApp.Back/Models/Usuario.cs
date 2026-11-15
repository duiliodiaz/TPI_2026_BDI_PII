using System;
using System.Collections.Generic;

namespace CineApp.Back.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string Rol { get; set; } = null!;
}
