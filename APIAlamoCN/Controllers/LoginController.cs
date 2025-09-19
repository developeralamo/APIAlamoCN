using APIAlamoCN.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APIAlamoCN.Models.Login;
using APIAlamoCN.Models.Response;
using APIAlamoCN.Models.Response.Login;
using APIAlamoCN.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace APIAlamoCN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> Logger;

        public LoginController(ILogger<LoginController> logger, LoginRepository repository, IConfiguration configuration)
        {
            Logger = logger;
            Repository = repository;
            Configuration = configuration;
        }

        public LoginRepository Repository { get; }
        public IConfiguration Configuration { get; }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Post([FromBody] UsuarioDTO credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.usuario) || string.IsNullOrWhiteSpace(credentials.password))
            {
                return BadRequest(new LoginResponse { mensaje = "Usuario y contraseña requeridos." });
            }

            var key = Configuration["key"];
            if (string.IsNullOrEmpty(key))
            {
                Logger.LogError("No se encontró la clave JWT en la configuración.");
                return StatusCode(500, new LoginResponse { mensaje = "Error de configuración del servidor." });
            }

            var errorMessage = await Repository.LoginWithJwt(credentials.usuario, credentials.password);
            Logger.LogInformation("LoginWithJwt devolvió: {m}", errorMessage ?? "[null – OK]");

            if (errorMessage != null)
            {
                return Unauthorized(new LoginResponse { mensaje = errorMessage });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, credentials.usuario)
            // Puedes agregar más claims aquí si es necesario
        }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var response = new LoginResponse
            {
                token = tokenHandler.WriteToken(token),
                expirationDate = tokenDescriptor.Expires
            };

            return Ok(response);
        }


    }
}