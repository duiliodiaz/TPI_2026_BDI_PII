namespace CineApp.Back.Security;

// Se carga desde la sección "Jwt" de la configuración.
// La clave (Jwt:Key) NO va en appsettings.json: se guarda en User Secrets.
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ExpiracionMinutos { get; set; } = 120;
}
