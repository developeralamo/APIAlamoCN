using APIAlamoCN.Repositories;
using Microsoft.Data.SqlClient;
using APIAlamoCN.Models.Response;
using APIAlamoCN.Models.Response.Login;
using System.Configuration;


namespace APIAlamoCN.Repositories
{
    public class LoginRepository : RepositoryBase
    {
        public LoginRepository(IConfiguration configuration, Serilog.ILogger logger) : base(configuration, logger)

        {

        }

        public async Task<string?> LoginWithJwt(string usuario, string password)
        {

            string query = $"SELECT * FROM USR_WSTUSH WHERE USR_WSTUSH_CODIGO = '{usuario}'";

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnectionString")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            return "El usuario no existe.";

                        }
                    }

                }

            }
            query = $"SELECT * FROM USR_WSTUSH WHERE USR_WSTUSH_CODIGO = '{usuario}' AND USR_WSTUSH_PASSWR = '{password}'";

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnectionString")))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            return "Contraseña incorrecta.";
                        }
                    }

                }
            }

            return null;
        }
    }
}
