using APIAlamoCN.Models.Response.Comprobante;
using Microsoft.Data.SqlClient;

namespace APIAlamoCN.Repositories
{
    public class CuentaCorrienteRepository : RepositoryBase
    {
        public CuentaCorrienteRepository(IConfiguration configuration, Serilog.ILogger logger) : base(configuration, logger)
        {
        }

    }
}