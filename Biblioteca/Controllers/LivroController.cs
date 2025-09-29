using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private readonly ILogger<LivroController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLivro")]

        public IEnumerable<Livro> GetLivro()
        {
            List<Livro> Livros = new List<Livro>();

            using (SqlConnection conection = new SqlConnection(StrConex))
            {

                string query = "SELECT * FROM Livro";
                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Livro Livro = new Livro()
                    {
                        Id = Convert.ToInt32(reader["Id_Livro"]),
                        Nome = reader["Nome_Livro"].ToString(),
                        Sobrenome = reader["Sobrenome_Livro"].ToString(),
                        RM = reader["RM_Livro"]?.ToString() ?? string.Empty,
                        Telefone = reader["Telefone_Livro"].ToString(),
                        Curso = reader["Curso"].ToString(),
                        Status = reader["Status_Livro"].ToString()
                    };
                    Livros.Add(Livro);
                }
                reader.Close();
            }
            return Livros;
        }

        [HttpGet("search/{searchTerm}")]
        public ActionResult SearchLivro(string searchTerm)
        {
            var Livros = new List<Livro>();

            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                connection.Open();

                bool isNumero = int.TryParse(searchTerm, out int rmInt);

                string query;
                SqlCommand command;

                if (isNumero)
                {
                    query = "SELECT * FROM Livro WHERE RM_Livro = @RM";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RM", rmInt);
                }
                else
                {
                    query = "SELECT * FROM Livro WHERE Nome_Livro LIKE @Nome";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nome", "%" + searchTerm + "%");
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Livro = new Livro
                        {
                            Id = Convert.ToInt32(reader["Id_Livro"]),
                            Nome = reader["Nome_Livro"] as string ?? string.Empty,
                            Sobrenome = reader["Sobrenome_Livro"] as string ?? string.Empty,
                            RM = reader["RM_Livro"]?.ToString() ?? string.Empty,
                            Telefone = reader["Telefone_Livro"]?.ToString() ?? string.Empty,
                            Curso = reader["Curso"]?.ToString() ?? string.Empty,
                            Status = reader["Status_Livro"]?.ToString() ?? string.Empty
                        };
                        Livros.Add(Livro);
                    }
                }
            }

            if (Livros.Count == 0)
                return NotFound();

            return Ok(Livros);
        }


        [HttpGet("{id}", Name = "GetLivroID")]

        public ActionResult GetLivroId(int id)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                string query = "SELECT * FROM Livro WHERE ID = @Id";
                SqlCommand comand = new SqlCommand(query, connection);
                comand.Parameters.AddWithValue("@Id", id);
                connection.Open();

                SqlDataReader reader = comand.ExecuteReader();

                if (reader.Read())
                {
                    Livro Livro = new Livro()
                    {
                        Id = Convert.ToInt32(reader["Id_Livro"]),
                        Nome = reader["Nome_Livro"].ToString(),
                        Sobrenome = reader["Sobrenome_Livro"].ToString(),
                        RM = reader["RM_Livro"]?.ToString() ?? string.Empty,
                        Telefone = reader["Telefone_Livro"].ToString(),
                        Curso = reader["Curso"].ToString(),
                        Status = reader["Status_Livro"].ToString()
                    };
                    reader.Close();

                    return Ok(Livro);
                }
                reader.Close();
            }
            return NotFound();
        }

        [HttpPost]

        public ActionResult CreateLivro(Livro Livro)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "INSERT INTO Livro (ISBN,Condicoes_Encardenacoes,Agencia_Catalogadora,Idioma_Catalogacao,Agencia_Transcricao,Agencia_Modificacao,Idioma_Texto ,Idioma_Resumo,Idioma_Legenda,Numero_CDD,Numero_Item_CDD,Numero_Chamada_Local,Numero_Item_Local,Numero_Chamada_Secundaria ,Nome_Livro,Subtitulo,Indicacao_Responsabilidade,Indicador_Artigo_Inicial,Numero_Edicao,Mencao_Responsabilidade_Edicao,Local_Publicacao,Editora,Ano_Publicacao,Paginas,Ilustracoes,Dimensoes,Material_Adicional,Titulo_Serie,Numero_Serie,Notas_Gerais,Nome_Pessoal_Assunto,Datas_Pessoal,Funcao_Pessoal,Topico,Titulo_Uniforme,Forma_Uniforme,Periodo_Historico,Localidade_Uniforme,Assunto_Termo,Forma_Termo,Periodo_Historico_Termo,Localidade_Termo,Informacao_Local,Status_Item,Status_Emprestimo ) VALUES (@Nome,@Sobrenome,@RM,@Telefone,@Curso,@Status)";
                SqlCommand comand = new SqlCommand(query, conection);
                comand.Parameters.AddWithValue("@Nome", Livro.Nome);
                comand.Parameters.AddWithValue("@Sobrenome", Livro.Sobrenome);
                comand.Parameters.AddWithValue("@RM", Livro.RM);
                comand.Parameters.AddWithValue("@Telefone", Livro.Telefone);
                comand.Parameters.AddWithValue("@Curso", Livro.Curso);
                comand.Parameters.AddWithValue("@Status", Livro.Status);
                conection.Open();
                int rowsAffected = comand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [HttpPut]

        public ActionResult UpdateLivro(int id, [FromBody] Livro Livro)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                string query = "UPDATE Livro SET Nome_Livro = @Nome, Sobrenome_Livro = @Sobrenome,RM_Livro = @RM,Telefone_Livro = @Telefone,Curso = @Curso,Status_Livro = @Status WHERE Id_Livro = @Id";
                SqlCommand comand = new SqlCommand(query, connection);
                comand.Parameters.AddWithValue("@Nome", Livro.Nome);
                comand.Parameters.AddWithValue("@Sobrenome", Livro.Sobrenome);
                comand.Parameters.AddWithValue("@RM", Livro.RM);
                comand.Parameters.AddWithValue("@Telefone", Livro.Telefone);
                comand.Parameters.AddWithValue("@Curso", Livro.Curso);
                comand.Parameters.AddWithValue("@Status", Livro.Status);
                comand.Parameters.AddWithValue("@Id", id);
                connection.Open();

                int rowsAffected = comand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }


    }
}

