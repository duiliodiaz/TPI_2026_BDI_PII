using CineApp.Back.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers;

public static class RespuestasError
{
    // Traduce el tipo de error del service al código HTTP correspondiente,
    // siempre con el mismo formato: { "mensaje": "..." }.
    public static IActionResult RespuestaError(this ControllerBase controller, TipoError error, string mensaje)
    {
        return error switch
        {
            TipoError.NoEncontrado => controller.NotFound(new { mensaje }),
            TipoError.Conflicto => controller.Conflict(new { mensaje }),
            TipoError.Interno => controller.StatusCode(StatusCodes.Status500InternalServerError, new { mensaje }),
            _ => controller.BadRequest(new { mensaje })
        };
    }
}
