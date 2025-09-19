using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Globalization;
using APIAlamoCN.Repositories;
using APIAlamoCN.Services;
using APIAlamoCN.Models.Response.Comprobante;
using APIAlamoCN.Models.FC;

namespace APIAlamoCN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NCController : ControllerBase
    {

        private Serilog.ILogger Logger { get; set; }
        public FCRepository Repository { get; }
        public IConfiguration Configuration { get; }

        public NCController(Serilog.ILogger logger, FCRepository repository, IConfiguration configuration)
        {
            Logger = logger;
            Repository = repository;
            Configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult<ComprobanteResponse>> PostFacturacion([FromBody] FCDTO payload)
        {
            ComprobanteResponse response = null;
            bool dioError = false;

            var ControllerName = "NC";
            string JsonBody = JsonConvert.SerializeObject(payload);
            Logger.Information("{ControllerName} - Body recibido: {JsonBody}", ControllerName, JsonBody);

     

            FieldMapper mapping = new FieldMapper();
            if (!mapping.LoadMappingFile(AppDomain.CurrentDomain.BaseDirectory + @"\Services\FieldMapFiles\NC.json"))
            {
                response = new ComprobanteResponse(new ComprobanteDTO((string?)payload.identificador
                , "400", "Error de configuracion", "No se encontro el archivo de configuracion del endpoint", null));

                return BadRequest(response);
            };


            string errorMessage = await Repository.ExecuteSqlInsertToTablaSAR(mapping.fieldMap,
                                                                            payload,
                                                                            payload.identificador,
                                                                            Configuration["FC:JobName"]);

            if (errorMessage != "")
            {
                dioError = true;

                Logger.Error("Error en FCRepository.ExecuteSqlInsertToTablaSAR: {Error}", errorMessage);

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
            ComprobanteResponse respuesta = await Repository.GetTransaccion(identificador, "SAR_FCRMVH");

            switch (respuesta.response.status)
            {
                case "404":
                    return NotFound(respuesta);
                   
                default:
                    return Ok(respuesta);
                   
            }

        }

    }
}