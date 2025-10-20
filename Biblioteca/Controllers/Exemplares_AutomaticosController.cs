using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Exemplares_AutomaticosController : Controller
    {

        private readonly ILogger<Exemplares_AutomaticosController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public Exemplares_AutomaticosController(ILogger<Exemplares_AutomaticosController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetExemplares")]
        public IEnumerable<Exemplares_Automaticos> GetExemplares()
        {
            List<Exemplares_Automaticos> Exemplares = new List<Exemplares_Automaticos>();
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "SELECT * FROM Exemplares_Automaticos";
                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Exemplares_Automaticos exemplares = new Exemplares_Automaticos()
                    {
                        Id = Convert.ToInt32(reader["Id_Exemplar_Auto"]),
                        Numero_Exemplares = Convert.ToInt32(reader["Numero_Exemplares"]),
                        Numero_Volume = Convert.ToInt32(reader["Numero_Volume"]),
                        Numero_Exemplares_Total = Convert.ToInt32(reader["Numero_Exemplares_Total"]),
                        Data_Aquisicao = reader["Data_Aquisicao"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["Data_Aquisicao"]))
                        : (DateOnly?)null,
                        Biblioteca_Depositaria = reader["Biblioteca_Depositaria"].ToString(),
                        Tipo_Aquisicao = reader["Tipo_Aquisicao"].ToString()


                    };
                    Exemplares.Add(exemplares);
                }
                reader.Close();
            }
            return Exemplares;
        }

    

    }
}
