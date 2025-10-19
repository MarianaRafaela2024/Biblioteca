using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
      
namespace Biblioteca.Controllers
    {
        [ApiController]
        [Route("[controller]")]
        public class EntidadeController : Controller
        {
            private readonly ILogger<EntidadeController> _logger;

            private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            public EntidadeController(ILogger<EntidadeController> logger)
            {
                _logger = logger;
            }

            [HttpGet(Name = "GetEntidade")]
            public IEnumerable<Entidade_Corporativa> GetEntidade()
            {
                List<Entidade_Corporativa> Entidades = new List<Entidade_Corporativa>();
                using (SqlConnection conection = new SqlConnection(StrConex))
                {
                    string query = "SELECT * FROM Entidade_Corporativa";
                    SqlCommand command = new SqlCommand(query, conection);
                    conection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Entidade_Corporativa Entidade = new Entidade_Corporativa()
                        {
                            Id = Convert.ToInt32(reader["Id_Entidade"]),
                            Nome_Entidade = reader["Nome_Entidade"]?.ToString() ?? string.Empty,
                            Subordinacao = reader["Subordinacao"]?.ToString() ?? string.Empty

                        };
                        Entidades.Add(Entidade);
                    }
                    reader.Close();
                }
                return Entidades;
            }

           

            [HttpGet("search")]
            public IActionResult Search(string? termo)
            {
                var Entidades = new List<object>();

                using (SqlConnection conection = new SqlConnection(StrConex))
                {
                    conection.Open();

                    string sql = @"
        SELECT 
            l.Id_Entidade, 
            l.Nome_Entidade, 
            l.Subtitulo, 
            l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, 
            l.ISBN, 
            l.Assunto_Termo,
            STRING_AGG(a.Nome_Entidade, ', ') AS Entidadees
        FROM Entidade l
        LEFT JOIN Entidade_Entidade la ON l.Id_Entidade = la.Id_Entidade
        LEFT JOIN Entidade a ON la.Id_Entidade = a.Id_Entidade
        WHERE (@termo IS NULL OR
               l.Nome_Entidade LIKE '%' + @termo + '%' OR
               l.Subtitulo LIKE '%' + @termo + '%' OR
               l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
               l.Assunto_Termo LIKE '%' + @termo + '%' OR
               CAST(l.Ano_Publicacao AS NVARCHAR) = @termo OR
               a.Nome_Entidade LIKE '%' + @termo + '%'
        )
        GROUP BY 
            l.Id_Entidade, l.Nome_Entidade, l.Subtitulo, l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, l.ISBN, l.Assunto_Termo
        ORDER BY l.Nome_Entidade";

                    using (SqlCommand cmd = new SqlCommand(sql, conection))
                    {
                        cmd.Parameters.AddWithValue("@termo", (object)termo ?? DBNull.Value);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Entidades.Add(new
                                {
                                    Id_Entidade = Convert.ToInt32(reader["Id_Entidade"]),
                                    Nome_Entidade = reader["Nome_Entidade"].ToString(),
                                    Subtitulo = reader["Subtitulo"].ToString(),
                                    Indicacao_Responsabilidade = reader["Indicacao_Responsabilidade"].ToString(),
                                    Ano_Publicacao = reader["Ano_Publicacao"] != DBNull.Value ? Convert.ToInt32(reader["Ano_Publicacao"]) : (int?)null,
                                    ISBN = reader["ISBN"].ToString(),
                                    Assunto_Termo = reader["Assunto_Termo"].ToString(),
                                    Entidadees = reader["Entidadees"]?.ToString()
                                });
                            }
                        }
                    }
                }
                return Ok(Entidades);
            }
        }
    }


