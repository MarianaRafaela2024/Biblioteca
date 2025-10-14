using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
        [ApiController]
        [Route("[controller]")]
    public class Livro_EntidadeController : Controller
    {

        
            private readonly ILogger<Livro_EntidadeController> _logger;

            private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            public Livro_EntidadeController(ILogger<Livro_EntidadeController> logger)
            {
                _logger = logger;
            }

            [HttpGet(Name = "GetLivro_Entidade")]
            public IEnumerable<Livro_Entidade> GetLivro_Entidade()
            {
                List<Livro_Entidade> Livro_Entidades = new List<Livro_Entidade>();
                using (SqlConnection conection = new SqlConnection(StrConex))
                {
                    string query = "SELECT * FROM Livro_Entidade";
                    SqlCommand command = new SqlCommand(query, conection);
                    conection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Livro_Entidade Livro_Entidade = new Livro_Entidade()
                        {
                            Id = Convert.ToInt32(reader["Id_Livro_Entidade"]),
                            Id_Livro = Convert.ToInt32(reader["Id_Livro"]),
                            Id_Entidade = Convert.ToInt32(reader["Id_Entidade"]),

                        };
                        Livro_Entidades.Add(Livro_Entidade);
                    }
                    reader.Close();
                }
                return Livro_Entidades;
            }

            


            [HttpGet("search")]
            public IActionResult Search(string? termo)
            {
                var Livro_Autors = new List<object>();

                using (SqlConnection conection = new SqlConnection(StrConex))
                {
                    conection.Open();

                    string sql = @"
        SELECT 
            l.Id_Livro_Autor, 
            l.Nome_Livro_Autor, 
            l.Subtitulo, 
            l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, 
            l.ISBN, 
            l.Assunto_Termo,
            STRING_AGG(a.Nome_Livro_Autor, ', ') AS Livro_Autores
        FROM Livro_Autor l
        LEFT JOIN Livro_Autor_Livro_Autor la ON l.Id_Livro_Autor = la.Id_Livro_Autor
        LEFT JOIN Livro_Autor a ON la.Id_Livro_Autor = a.Id_Livro_Autor
        WHERE (@termo IS NULL OR
               l.Nome_Livro_Autor LIKE '%' + @termo + '%' OR
               l.Subtitulo LIKE '%' + @termo + '%' OR
               l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
               l.Assunto_Termo LIKE '%' + @termo + '%' OR
               CAST(l.Ano_Publicacao AS NVARCHAR) = @termo OR
               a.Nome_Livro_Autor LIKE '%' + @termo + '%'
        )
        GROUP BY 
            l.Id_Livro_Autor, l.Nome_Livro_Autor, l.Subtitulo, l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, l.ISBN, l.Assunto_Termo
        ORDER BY l.Nome_Livro_Autor";

                    using (SqlCommand cmd = new SqlCommand(sql, conection))
                    {
                        cmd.Parameters.AddWithValue("@termo", (object)termo ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Livro_Autors.Add(new
                                {
                                    Id_Livro_Autor = Convert.ToInt32(reader["Id_Livro_Autor"]),
                                    Nome_Livro_Autor = reader["Nome_Livro_Autor"].ToString(),
                                    Subtitulo = reader["Subtitulo"].ToString(),
                                    Indicacao_Responsabilidade = reader["Indicacao_Responsabilidade"].ToString(),
                                    Ano_Publicacao = reader["Ano_Publicacao"] != DBNull.Value ? Convert.ToInt32(reader["Ano_Publicacao"]) : (int?)null,
                                    ISBN = reader["ISBN"].ToString(),
                                    Assunto_Termo = reader["Assunto_Termo"].ToString(),
                                    Livro_Autores = reader["Livro_Autores"]?.ToString()
                                });
                            }
                        }
                    }
                }

                return Ok(Livro_Autors);
            }





           



        }

        


    }






