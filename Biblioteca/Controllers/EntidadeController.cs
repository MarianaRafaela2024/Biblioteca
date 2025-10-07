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

            //[HttpGet("search/{searchTerm}")]
            //public ActionResult SearchEntidade(string searchTerm)
            //{
            //    var Entidades = new List<Entidade>();

            //    using (SqlConnection connection = new SqlConnection(StrConex))
            //    {
            //        connection.Open();

            //        bool isNumero = int.TryParse(searchTerm, out int rmInt);

            //        string query;
            //        SqlCommand command;

            //        if (isNumero)
            //        {
            //            query = "SELECT * FROM Entidade WHERE RM_Entidade = @RM";
            //            command = new SqlCommand(query, connection);
            //            command.Parameters.AddWithValue("@RM", rmInt);
            //        }
            //        else
            //        {
            //            query = "SELECT * FROM Entidade WHERE Nome_Entidade LIKE @Nome";
            //            command = new SqlCommand(query, connection);
            //            command.Parameters.AddWithValue("@Nome", "%" + searchTerm + "%");
            //        }

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var Entidade = new Entidade
            //                {
            //                    Id = Convert.ToInt32(reader["Id_Entidade"]),
            //                    Nome = reader["Nome_Entidade"] as string ?? string.Empty,
            //                    Sobrenome = reader["Sobrenome_Entidade"] as string ?? string.Empty,
            //                    RM = reader["RM_Entidade"]?.ToString() ?? string.Empty,
            //                    Telefone = reader["Telefone_Entidade"]?.ToString() ?? string.Empty,
            //                    Curso = reader["Curso"]?.ToString() ?? string.Empty,
            //                    Status = reader["Status_Entidade"]?.ToString() ?? string.Empty
            //                };
            //                Entidades.Add(Entidade);
            //            }
            //        }
            //    }

            //    if (Entidades.Count == 0)
            //        return NotFound();

            //    return Ok(Entidades);
            //}


            //[HttpGet("{id}", Name = "GetEntidadeID")]

            //public ActionResult GetEntidadeId(int id)
            //{
            //    using (SqlConnection connection = new SqlConnection(StrConex))
            //    {
            //        string query = "SELECT * FROM Entidade WHERE ID = @Id";
            //        SqlCommand comand = new SqlCommand(query, connection);
            //        comand.Parameters.AddWithValue("@Id", id);
            //        connection.Open();

            //        SqlDataReader reader = comand.ExecuteReader();

            //        if (reader.Read())
            //        {
            //            Entidade Entidade = new Entidade()
            //            {
            //                Id = Convert.ToInt32(reader["Id_Entidade"]),
            //                Nome = reader["Nome_Entidade"].ToString(),
            //                Sobrenome = reader["Sobrenome_Entidade"].ToString(),
            //                RM = reader["RM_Entidade"]?.ToString() ?? string.Empty,
            //                Telefone = reader["Telefone_Entidade"].ToString(),
            //                Curso = reader["Curso"].ToString(),
            //                Status = reader["Status_Entidade"].ToString()
            //            };
            //            reader.Close();

            //            return Ok(Entidade);
            //        }
            //        reader.Close();
            //    }
            //    return NotFound();
            //}


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





            [HttpPost]
            public ActionResult CreateEntidade(Entidade_Corporativa Entidade)
            {
                using (SqlConnection conection = new SqlConnection(StrConex))
                {
                    string query = "INSERT INTO Entidade_Corporativa (Nome_Entidade,Subordinacao) VALUES (@Nome_Entidade,@Subordinacao)";

                    SqlCommand comand = new SqlCommand(query, conection);

                    comand.Parameters.AddWithValue("@Nome_Entidade", Entidade.Nome_Entidade);
                    comand.Parameters.AddWithValue("@Subordinacao", Entidade.Subordinacao);

                    conection.Open();
                    int rowsAffected = comand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }



        }

        //[HttpPut("{id}")]
        //[HttpPut]

        //public ActionResult UpdateEntidade(int id, [FromBody] Entidade Entidade)
        //{
        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        string query = "UPDATE Entidade SET Nome_Entidade = @Nome, Sobrenome_Entidade = @Sobrenome,RM_Entidade = @RM,Telefone_Entidade = @Telefone,Curso = @Curso,Status_Entidade = @Status WHERE Id_Entidade = @Id";
        //        SqlCommand comand = new SqlCommand(query, connection);
        //        comand.Parameters.AddWithValue("@Nome", Entidade.Nome);
        //        comand.Parameters.AddWithValue("@Sobrenome", Entidade.Sobrenome);
        //        comand.Parameters.AddWithValue("@RM", Entidade.RM);
        //        comand.Parameters.AddWithValue("@Telefone", Entidade.Telefone);
        //        comand.Parameters.AddWithValue("@Curso", Entidade.Curso);
        //        comand.Parameters.AddWithValue("@Status", Entidade.Status);
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


