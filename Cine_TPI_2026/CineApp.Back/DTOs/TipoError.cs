namespace CineApp.Back.DTOs;

// Indica por qué falló una operación de un service, para que el controller
// responda con el código HTTP correcto.
public enum TipoError
{
    Ninguno,        // salió bien
    Validacion,     // 400: datos inválidos o que referencian algo inexistente
    NoEncontrado,   // 404: el registro del id de la URL no existe
    Conflicto,      // 409: choca con otro registro (DNI, email o nombre repetido)
    Interno         // 500: no se pudo guardar en la base de datos
}
