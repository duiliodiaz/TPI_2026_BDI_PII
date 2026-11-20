using CineApp.Back.DTOs;
using CineApp.Back.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.NombreUsuario) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { mensaje = "Debe ingresar usuario y contraseña." });

        var usuario = await _service.Login(request);

        if (usuario == null)
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });

        return Ok(usuario);
    }
}
