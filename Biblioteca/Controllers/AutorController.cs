using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutorController : Controller
    {
        private readonly ILogger<AutorController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public AutorController(ILogger<AutorController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAutor")]
        public IEnumerable<Autor> GetAutor()
        {
            List<Autor> Autors = new List<Autor>();
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "SELECT * FROM Autor";
                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Autor Autor = new Autor()
                    {
                        Id = Convert.ToInt32(reader["Id_Autor"]),
                        Nome_Autor = reader["Nome_Autor"]?.ToString() ?? string.Empty,
                        Numero = reader["Numero"]?.ToString() ?? string.Empty,
                        Datas = reader["Datas"]?.ToString() ?? string.Empty,
                        Funcao = reader["Funcao"]?.ToString() ?? string.Empty,
                        Tipo_Autor = reader["Tipo_Autor"]?.ToString() ?? string.Empty,
                        
                    };
                    Autors.Add(Autor);
                }
                reader.Close();
            }
            return Autors;
        }

       

        [HttpGet("search")]
        public IActionResult Search(string? termo)
        {
            var Autors = new List<object>();

            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                conection.Open();

                string sql = @"
        SELECT 
            l.Id_Autor, 
            l.Nome_Autor, 
            l.Subtitulo, 
            l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, 
            l.ISBN, 
            l.Assunto_Termo,
            STRING_AGG(a.Nome_Autor, ', ') AS Autores
        FROM Autor l
        LEFT JOIN Autor_Autor la ON l.Id_Autor = la.Id_Autor
        LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
        WHERE (@termo IS NULL OR
               l.Nome_Autor LIKE '%' + @termo + '%' OR
               l.Subtitulo LIKE '%' + @termo + '%' OR
               l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
               l.Assunto_Termo LIKE '%' + @termo + '%' OR
               CAST(l.Ano_Publicacao AS NVARCHAR) = @termo OR
               a.Nome_Autor LIKE '%' + @termo + '%'
        )
        GROUP BY 
            l.Id_Autor, l.Nome_Autor, l.Subtitulo, l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, l.ISBN, l.Assunto_Termo
        ORDER BY l.Nome_Autor";

                using (SqlCommand cmd = new SqlCommand(sql, conection))
                {
                    cmd.Parameters.AddWithValue("@termo", (object)termo ?? DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Autors.Add(new
                            {
                                Id_Autor = Convert.ToInt32(reader["Id_Autor"]),
                                Nome_Autor = reader["Nome_Autor"].ToString(),
                                Subtitulo = reader["Subtitulo"].ToString(),
                                Indicacao_Responsabilidade = reader["Indicacao_Responsabilidade"].ToString(),
                                Ano_Publicacao = reader["Ano_Publicacao"] != DBNull.Value ? Convert.ToInt32(reader["Ano_Publicacao"]) : (int?)null,
                                ISBN = reader["ISBN"].ToString(),
                                Assunto_Termo = reader["Assunto_Termo"].ToString(),
                                Autores = reader["Autores"]?.ToString()
                            });
                        }
                    }
                }
            }

            return Ok(Autors);
        }





       


        
    }

    //[HttpPut("{id}")]
    //[HttpPut]

    //public ActionResult UpdateAutor(int id, [FromBody] Autor Autor)
    //{
    //    using (SqlConnection connection = new SqlConnection(StrConex))
    //    {
    //        string query = "UPDATE Autor SET Nome_Autor = @Nome, Sobrenome_Autor = @Sobrenome,RM_Autor = @RM,Telefone_Autor = @Telefone,Curso = @Curso,Status_Autor = @Status WHERE Id_Autor = @Id";
    //        SqlCommand comand = new SqlCommand(query, connection);
    //        comand.Parameters.AddWithValue("@Nome", Autor.Nome);
    //        comand.Parameters.AddWithValue("@Sobrenome", Autor.Sobrenome);
    //        comand.Parameters.AddWithValue("@RM", Autor.RM);
    //        comand.Parameters.AddWithValue("@Telefone", Autor.Telefone);
    //        comand.Parameters.AddWithValue("@Curso", Autor.Curso);
    //        comand.Parameters.AddWithValue("@Status", Autor.Status);
    //        comand.Parameters.AddWithValue("@Id", id);
    //        connection.Open();

    //        int rowsAffected = comand.ExecuteNonQuery();

    //        if (rowsAffected > 0)
    //        {
    //            return Ok();
    //        }
    //    }
    //    return NotFound();
    //}


}


