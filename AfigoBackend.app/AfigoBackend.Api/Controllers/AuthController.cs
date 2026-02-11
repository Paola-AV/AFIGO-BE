using AfigoBackend.Aplication.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AfigoBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthInterface _service;
    public AuthController(IAuthInterface service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        
        var correo = dto.Correo?.Trim().ToLowerInvariant() ?? string.Empty;
        var nombreUsuario = string.IsNullOrWhiteSpace(dto.NombreUsuario)
            ? correo     
            : dto.NombreUsuario.Trim();

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Correo y contraseña son obligatorios." });

        try
        {
            await _service.RegistrarAsync(
                correo,
                dto.Nombre?.Trim() ?? string.Empty,
                dto.Password,
                nombreUsuario,
                dto.IsAdmin ? 1 : 0, 
                ct);

            return Ok(new { message = "Usuario registrado" });
        }
        catch (InvalidOperationException ex)
        {
            
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var userKey = dto.CorreoOUsuario?.Trim().ToLowerInvariant() ?? string.Empty;
        var ok = await _service.LoginAsync(userKey, dto.Password, ct);

        return ok
            ? Ok(new { message = "Credenciales válidas" })   
            : Unauthorized(new { error = "Usuario o contraseña inválidos" });
    }
}


public record RegisterDto(
    string Correo,
    string Nombre,
    string Password,
    string? NombreUsuario,
    bool IsAdmin 
);

public record LoginDto(string CorreoOUsuario, string Password);