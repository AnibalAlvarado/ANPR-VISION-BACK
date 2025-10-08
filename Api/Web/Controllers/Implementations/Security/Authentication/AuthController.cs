using Business.Interfaces.Security.Authentication;
using Entity.Dtos.Security;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Implementations.Security.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticationBusiness _authBusiness;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserAuthenticationBusiness authBusiness, ILogger<AuthController> logger)
        {
            _authBusiness = authBusiness;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<UserResponseDto>(
                    null, false, "Datos inválidos",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            try
            {
                var user = await _authBusiness.AuthenticateAsync(request.Username, request.Password);
                if (user == null)
                    return Unauthorized(new ApiResponse<UserResponseDto>(null, false, "Credenciales incorrectas", null));

                return Ok(new ApiResponse<UserResponseDto>(user, true, "Inicio de sesión exitoso", null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en autenticación");
                return StatusCode(500, new ApiResponse<UserResponseDto>(null, false, "Error interno del servidor", null));
            }
        }

        // (Opcional) futuros endpoints:
        // [HttpPost("refresh")]
        // [HttpPost("logout")]
        // [HttpPost("google-login")]
    }
}
