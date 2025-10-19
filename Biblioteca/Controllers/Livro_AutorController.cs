using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class Livro_AutorController : Controller
    {
        private readonly ILogger<Livro_AutorController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public Livro_AutorController(ILogger<Livro_AutorController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLivro_Livro_Autor")]
        public IEnumerable<Livro_Autor> GetLivro_Livro_Autor()
        {
            List<Livro_Autor> Livro_Autors = new List<Livro_Autor>();
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "SELECT * FROM Livro_Autor";
                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Livro_Autor Livro_Autor = new Livro_Autor()
                    {
                        Id = Convert.ToInt32(reader["Id_Livro_Autor"]),
                        Id_Livro = Convert.ToInt32(reader["Id_Livro"]),
                        Id_Autor = Convert.ToInt32(reader["Id_Autor"]),

                    };
                    Livro_Autors.Add(Livro_Autor);
                }
                reader.Close();
            }
            return Livro_Autors;
        } 
    }
}




