using AfigoBackend.Aplication.Abstractions.Interfaces;
using AfigoBackend.Domain.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
        var nombreUsuario = dto.NombreUsuario?.Trim().ToLowerInvariant() ?? string.Empty;

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
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var user = await _service.LoginAsync(dto.CorreoOUsuario, dto.Password, ct);
        if (user is null)
            return Unauthorized(new { error = "Usuario o contraseña inválidos" });

        // Construye los claims del usuario
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.NombreDeUsuario),
        new Claim(ClaimTypes.GivenName, user.Nombre ?? string.Empty),
        new Claim(ClaimTypes.Email, user.Correo ?? string.Empty),
        new Claim("is_admin", user.UsuarioAdmin.ToString())
    };

        if (user.UsuarioAdmin == 1)
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Propiedades de la cookie (puedes ajustar expiración aquí también)
        var authProps = new AuthenticationProperties
        {
            IsPersistent = false, // true si quieres "recordarme" (persistente entre cierres de navegador)
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) // opcional; si no, toma la de options
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

        var resp = new UserLoginDto(user.UserId, user.Nombre ?? string.Empty, user.UsuarioAdmin);
        return Ok(resp);
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Sesión cerrada" });
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

public record UserLoginDto(
            int UserId,
            string Nombre,
            int UsuarioAdmin
        );