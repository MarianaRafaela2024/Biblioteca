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

        //[HttpGet("search/{searchTerm}")]
        //public ActionResult SearchLivro_Autor(string searchTerm)
        //{
        //    var Livro_Autors = new List<Livro_Autor>();

        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        connection.Open();

        //        bool isNumero = int.TryParse(searchTerm, out int rmInt);

        //        string query;
        //        SqlCommand command;

        //        if (isNumero)
        //        {
        //            query = "SELECT * FROM Livro_Autor WHERE RM_Livro_Autor = @RM";
        //            command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@RM", rmInt);
        //        }
        //        else
        //        {
        //            query = "SELECT * FROM Livro_Autor WHERE Nome_Livro_Autor LIKE @Nome";
        //            command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@Nome", "%" + searchTerm + "%");
        //        }

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var Livro_Autor = new Livro_Autor
        //                {
        //                    Id = Convert.ToInt32(reader["Id_Livro_Autor"]),
        //                    Nome = reader["Nome_Livro_Autor"] as string ?? string.Empty,
        //                    Sobrenome = reader["Sobrenome_Livro_Autor"] as string ?? string.Empty,
        //                    RM = reader["RM_Livro_Autor"]?.ToString() ?? string.Empty,
        //                    Telefone = reader["Telefone_Livro_Autor"]?.ToString() ?? string.Empty,
        //                    Curso = reader["Curso"]?.ToString() ?? string.Empty,
        //                    Status = reader["Status_Livro_Autor"]?.ToString() ?? string.Empty
        //                };
        //                Livro_Autors.Add(Livro_Autor);
        //            }
        //        }
        //    }

        //    if (Livro_Autors.Count == 0)
        //        return NotFound();

        //    return Ok(Livro_Autors);
        //}


        //[HttpGet("{id}", Name = "GetLivro_AutorID")]

        //public ActionResult GetLivro_AutorId(int id)
        //{
        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        string query = "SELECT * FROM Livro_Autor WHERE ID = @Id";
        //        SqlCommand comand = new SqlCommand(query, connection);
        //        comand.Parameters.AddWithValue("@Id", id);
        //        connection.Open();

        //        SqlDataReader reader = comand.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            Livro_Autor Livro_Autor = new Livro_Autor()
        //            {
        //                Id = Convert.ToInt32(reader["Id_Livro_Autor"]),
        //                Nome = reader["Nome_Livro_Autor"].ToString(),
        //                Sobrenome = reader["Sobrenome_Livro_Autor"].ToString(),
        //                RM = reader["RM_Livro_Autor"]?.ToString() ?? string.Empty,
        //                Telefone = reader["Telefone_Livro_Autor"].ToString(),
        //                Curso = reader["Curso"].ToString(),
        //                Status = reader["Status_Livro_Autor"].ToString()
        //            };
        //            reader.Close();

        //            return Ok(Livro_Autor);
        //        }
        //        reader.Close();
        //    }
        //    return NotFound();
        //}


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





        [HttpPost]
        public ActionResult CreateLivro_Autor([FromBody] LivroAutorRequest request)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {

                //Pegando Id do livro
                conection.Open();
                string selectLivro = "SELECT Id_Livro FROM Livro WHERE ISBN = @ISBN";

                SqlCommand cmdLivro = new SqlCommand(selectLivro, conection);
                cmdLivro.Parameters.AddWithValue("@ISBN", request.ISBN);
                SqlDataReader reader = cmdLivro.ExecuteReader();
                int idLivro = Convert.ToInt32(reader["Id_Livro"]);
                reader.Close();
                conection.Close();

                //Pegando o Id do autor
                conection.Open();
                string selectAutor = "SELECT Id_Autor FROM Autor WHERE Nome_Autor = @Nome_Autor";
                SqlCommand cmdAutor = new SqlCommand(selectAutor, conection);
                cmdAutor.Parameters.AddWithValue("@Nome_Autor", request.Nome_Autor);

                SqlDataReader reader2 = cmdLivro.ExecuteReader();
                int idAutor = Convert.ToInt32(reader2["Id_Autor"]);
                reader2.Close();
                conection.Close();

                //Inserindo os id na tabela Livro_Autor
                conection.Open();
                string query = "INSERT INTO Livro_Autor (Id_Livro,Id_Autor) VALUES (@Id_Livro,@Id_Autor)";
                SqlCommand comand = new SqlCommand(query, conection);
                comand.Parameters.AddWithValue("@Id_Autor", idAutor);
                comand.Parameters.AddWithValue("@Id_Livro", idLivro);

                int rowsAffected = comand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
                conection.Close();
            }
            return BadRequest();

        }

        //[HttpPost]
        //public ActionResult CreateLivro_Autor([FromBody] LivroAutorRequest request)
        //{
        //    if (string.IsNullOrEmpty(request.ISBN) || string.IsNullOrEmpty(request.Nome_Autor))
        //    {
        //        return BadRequest(new { message = "ISBN e Nome_Autor são obrigatórios" });
        //    }

        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        connection.Open();

        //        // Pegando Id do livro
        //        string selectLivro = "SELECT Id_Livro FROM Livro WHERE ISBN = @ISBN";
        //        SqlCommand cmdLivro = new SqlCommand(selectLivro, connection);
        //        cmdLivro.Parameters.AddWithValue("@ISBN", request.ISBN);

        //        object resultLivro = cmdLivro.ExecuteScalar();
        //        if (resultLivro == null)
        //        {
        //            return NotFound(new { message = $"Livro não encontrado com ISBN: {request.ISBN}" });
        //        }
        //        int idLivro = Convert.ToInt32(resultLivro);

        //        // Pegando Id do autor
        //        string selectAutor = "SELECT Id_Autor FROM Autor WHERE Nome_Autor = @Nome_Autor";
        //        SqlCommand cmdAutor = new SqlCommand(selectAutor, connection);
        //        cmdAutor.Parameters.AddWithValue("@Nome_Autor", request.Nome_Autor);

        //        object resultAutor = cmdAutor.ExecuteScalar();
        //        if (resultAutor == null)
        //        {
        //            return NotFound(new { message = $"Autor não encontrado com nome: {request.Nome_Autor}" });
        //        }
        //        int idAutor = Convert.ToInt32(resultAutor);

        //        // Verificar se relacionamento já existe
        //        string checkQuery = "SELECT COUNT(*) FROM Livro_Autor WHERE Id_Livro = @Id_Livro AND Id_Autor = @Id_Autor";
        //        SqlCommand cmdCheck = new SqlCommand(checkQuery, connection);
        //        cmdCheck.Parameters.AddWithValue("@Id_Livro", idLivro);
        //        cmdCheck.Parameters.AddWithValue("@Id_Autor", idAutor);

        //        int exists = Convert.ToInt32(cmdCheck.ExecuteScalar());
        //        if (exists > 0)
        //        {
        //            return Conflict(new { message = "Este relacionamento já existe" });
        //        }

        //        // Inserindo o relacionamento
        //        string query = "INSERT INTO Livro_Autor (Id_Livro, Id_Autor) VALUES (@Id_Livro, @Id_Autor)";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@Id_Livro", idLivro);
        //        command.Parameters.AddWithValue("@Id_Autor", idAutor);

        //        int rowsAffected = command.ExecuteNonQuery();

        //        if (rowsAffected > 0)
        //        {
        //            return Ok(new
        //            {
        //                message = "Relacionamento criado com sucesso",
        //                Id_Livro = idLivro,
        //                Id_Autor = idAutor
        //            });
        //        }
        //    }

        //    return BadRequest(new { message = "Erro ao criar relacionamento" });
        //}

    }

    //[HttpPut("{id}")]
    //[HttpPut]

    //public ActionResult UpdateLivro_Autor(int id, [FromBody] Livro_Autor Livro_Autor)
    //{
    //    using (SqlConnection connection = new SqlConnection(StrConex))
    //    {
    //        string query = "UPDATE Livro_Autor SET Nome_Livro_Autor = @Nome, Sobrenome_Livro_Autor = @Sobrenome,RM_Livro_Autor = @RM,Telefone_Livro_Autor = @Telefone,Curso = @Curso,Status_Livro_Autor = @Status WHERE Id_Livro_Autor = @Id";
    //        SqlCommand comand = new SqlCommand(query, connection);
    //        comand.Parameters.AddWithValue("@Nome", Livro_Autor.Nome);
    //        comand.Parameters.AddWithValue("@Sobrenome", Livro_Autor.Sobrenome);
    //        comand.Parameters.AddWithValue("@RM", Livro_Autor.RM);
    //        comand.Parameters.AddWithValue("@Telefone", Livro_Autor.Telefone);
    //        comand.Parameters.AddWithValue("@Curso", Livro_Autor.Curso);
    //        comand.Parameters.AddWithValue("@Status", Livro_Autor.Status);
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




