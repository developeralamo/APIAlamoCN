using APIAlamoCN.Models.Response.Pdf;


namespace APIAlamoCN.Repositories
{
    public class ComprobanteRepository : RepositoryBase
    {
        public ComprobanteRepository(IConfiguration configuration, Serilog.ILogger logger) : base(configuration, logger)
        { }


        public async Task<PdfDTO?> GetPdfPath(string empresaFormulario,string codigoFormulario, int numeroFormulario)
        {





            return await ExecuteStoredProcedure<PdfDTO>("ALM_GetPdfForAPI",
                                                                            new Dictionary<string, object>{
                                                                                { "@Codemp", empresaFormulario },
                                                                                { "@Codfor", codigoFormulario },
                                                                                { "@Nrofor", numeroFormulario}
                                                                            });


        }

    }
}
