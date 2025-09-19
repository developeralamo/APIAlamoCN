using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Globalization;
using APIAlamoCN.Models.Pedidos;
using APIAlamoCN.Repositories;
using APIAlamoCN.Services;
using APIAlamoCN.Models.Response.Comprobante;
using APIAlamoCN.Models.CuentaCorriente;
using APIAlamoCN.Models.Cliente;

namespace APIAlamoCN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {

        private Serilog.ILogger Logger { get; set; }
        public ClientesRepository Repository { get; }
        public IConfiguration Configuration { get; }

        public ClientesController(Serilog.ILogger logger, ClientesRepository repository, IConfiguration configuration)
        {
            Logger = logger;
            Repository = repository;
            Configuration = configuration;
        }

        

        [HttpPost]
        public async Task<ActionResult<ComprobanteResponse>> PostFacturacion([FromBody] ClienteDTO payload)
        {
            ComprobanteResponse response = null;
            bool dioError = false;

            var ControllerName = "Clientes";
            string JsonBody = JsonConvert.SerializeObject(payload);
            Logger.Information("{ControllerName} - Body recibido: {JsonBody}", ControllerName, JsonBody);

     

            FieldMapper mapping = new FieldMapper();
            if (!mapping.LoadMappingFile(AppDomain.CurrentDomain.BaseDirectory + @"\Services\FieldMapFiles\Clientes.json"))
            {
                response = new ComprobanteResponse(new ComprobanteDTO((string?)payload.identificador
                , "400", "Error de configuracion", "No se encontro el archivo de configuracion del endpoint", null));

                return BadRequest(response);
            };


            string errorMessage = await Repository.ExecuteSqlInsertToTablaSAR(mapping.fieldMap,
                                                                            payload,
                                                                            payload.identificador,
                                                                            Configuration["Clientes:JobName"]);
            if (errorMessage != "")
            {
                dioError = true;

                Logger.Error("Error en ClientesRepository.ExecuteSqlInsertToTablaSAR: {Error}", errorMessage);

                string userFriendlyMessage = "Ocurrió un error al procesar su solicitud. Por favor, intente nuevamente o contacte a soporte.";

                response = new ComprobanteResponse(new ComprobanteDTO(
                    Convert.ToString(payload.identificador, CultureInfo.CreateSpecificCulture("en-GB")),
                    "400",
                    "Bad Request",
                    userFriendlyMessage,
                    null
                ));
            }
            else
            {
                response = new ComprobanteResponse(new ComprobanteDTO(Convert.ToString(payload.identificador, CultureInfo.CreateSpecificCulture("en-GB")), "200", "OK", errorMessage, null));
            };


            JsonBody = JsonConvert.SerializeObject(response);
            Logger.Information("{ControllerName} - Respuesta: {JsonBody}", ControllerName, JsonBody);

            if (dioError)
            {
                return BadRequest(response);
            }

            return Ok(response);

        }

        [HttpGet]
        [Route("{identificador}")]

        public async Task<ActionResult<ComprobanteResponse>> GetFacturacion(string identificador)
        {
            ComprobanteResponse respuesta = await Repository.GetTransaccion(identificador, "SAR_VTMCLH");

            switch (respuesta.response.status)
            {
                case "404":
                    return NotFound(respuesta);
                   
                default:
                    return Ok(respuesta);
                   
            }

        }


        [HttpGet]
        [Route("cliente/{identificador}")]
        public async Task<ActionResult<ClienteDTO>> GetClienteById(string identificador)
        {
            string ControllerName = "Clientes";
            Logger.Information($"{ControllerName} - Se recibió consulta de Cliente. ID: {identificador}");

            List<ClienteDTO?> respuesta = await Repository.ExecuteStoredProcedureList<ClienteDTO>(
                                           "Alm_GetClientesForAPI", new Dictionary<string, object>
                                           {
                                      { "@numeroCliente", identificador }
                                           });

            if (respuesta == null || respuesta.Count == 0)
            {
                return NotFound(new { message = $"Cliente con identificador {identificador} no encontrado" });
            }
            return Ok(respuesta);
        }

    }
}