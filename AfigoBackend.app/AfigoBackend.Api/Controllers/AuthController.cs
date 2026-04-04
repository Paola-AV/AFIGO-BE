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

    private readonly ITrabajadorInterface _trabajadorService;
    public AuthController(IAuthInterface service, ITrabajadorInterface trabajadorService) {
        _service = service;
        _trabajadorService = trabajadorService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        
        var correo = dto.Correo?.Trim().ToLowerInvariant() ?? string.Empty;
        var nombreUsuario = dto.NombreUsuario?.Trim().ToLowerInvariant() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Correo y contraseña son obligatorios." });

        try
        {
            await _service.RegistrarUsuarioTrabajadorAsync(
                correo,
                dto.Nombre?.Trim() ?? string.Empty,
                dto.Password,
                nombreUsuario,
                dto.IsAdmin ? 1 : 0, 
                dto.FechaInicio,
                dto.VacacionesDisponibles,
                dto.Vendedor.HasValue && dto.Vendedor.Value ? 1 : 0,
                dto.NombreVendedor?.Trim() ?? string.Empty,
                dto.Sede ?? string.Empty,
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

        var trabajador = await _trabajadorService.GetByUsuarioIdAsync(user.UserId);
        var esVendedor = trabajador?.Vendedor ?? 0;
        var nombreVendedor = trabajador?.NombreVendedor ?? "";
        var sede = trabajador?.Sede;
        var resp = new UserLoginDto(user.UserId, user.Nombre ?? string.Empty, user.UsuarioAdmin, esVendedor, nombreVendedor, sede);
        return Ok(resp);
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Sesión cerrada" });
    }


    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto, CancellationToken ct)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
            return BadRequest(new { error = "Contraseña actual y nueva son obligatorias." });
        
      
        try
        {
            await _service.ChangePasswordAsync(dto.UserId, dto.CurrentPassword, dto.NewPassword, ct);

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("change-password/force")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePasswordForced([FromBody] ChangePasswordForcedRequest dto, CancellationToken ct)
    {
        
        try
        {
            await _service.ChangePasswordAsyncForce(dto.UserId,dto.NewPassword, ct);

            return Ok(new { message = "Contraseña actualizada correctamente." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

}

public class ChangePasswordForcedRequest
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
}


public record RegisterDto(
    string Correo,
    string Nombre,
    string Password,
    string? NombreUsuario,
    bool IsAdmin,
    DateOnly FechaInicio,
    decimal VacacionesDisponibles,
    bool? Vendedor,
    string? NombreVendedor,
    string? Sede
);

public record LoginDto(string CorreoOUsuario, string Password);

public record UserLoginDto(
            int UserId,
            string Nombre,
            int UsuarioAdmin,
            int? Vendedor, 
            string NombreVendedor,
            string Sede
        );

public record ChangePasswordDto(int UserId,string CurrentPassword, string NewPassword);