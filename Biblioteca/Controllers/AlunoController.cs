using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunoController : Controller
    {
        private readonly ILogger<AlunoController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public AlunoController(ILogger<AlunoController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetAlunos")]

        public IEnumerable<Aluno> GetAluno()
        {
            List<Aluno> alunos = new List<Aluno>();

            using (SqlConnection conection = new SqlConnection(StrConex))
            {

                string query = "SELECT * FROM Aluno";
                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Aluno aluno = new Aluno()
                    {
                        Id = Convert.ToInt32(reader["Id_Aluno"]),
                        Nome = reader["Nome_Aluno"].ToString(),
                        Sobrenome = reader["Sobrenome_Aluno"].ToString(),
                        RM = reader["RM_Aluno"]?.ToString() ?? string.Empty,
                        Telefone = reader["Telefone_Aluno"].ToString(),
                        Curso = reader["Curso"].ToString(),
                        Status = reader["Status_Aluno"].ToString()
                    };
                    alunos.Add(aluno);
                }
                reader.Close();
            }
            return alunos;
        }

        [HttpGet("search/{searchTerm}")]
        public ActionResult SearchAluno(string searchTerm)
        {
            var alunos = new List<Aluno>();

            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                connection.Open();

                bool isNumero = int.TryParse(searchTerm, out int rmInt);

                string query;
                SqlCommand command;

                if (isNumero)
                {
                    query = "SELECT * FROM Aluno WHERE RM_Aluno = @RM";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RM", rmInt);
                }
                else
                {
                    query = "SELECT * FROM Aluno WHERE Nome_Aluno LIKE @Nome";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nome", "%" + searchTerm + "%");
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var aluno = new Aluno
                        {
                            Id = Convert.ToInt32(reader["Id_Aluno"]),
                            Nome = reader["Nome_Aluno"] as string ?? string.Empty,
                            Sobrenome = reader["Sobrenome_Aluno"] as string ?? string.Empty,
                            RM = reader["RM_Aluno"]?.ToString() ?? string.Empty,
                            Telefone = reader["Telefone_Aluno"]?.ToString() ?? string.Empty,
                            Curso = reader["Curso"]?.ToString() ?? string.Empty,
                            Status = reader["Status_Aluno"]?.ToString() ?? string.Empty
                        };
                        alunos.Add(aluno);
                    }
                }
            }

            if (alunos.Count == 0)
                return NotFound();

            return Ok(alunos);
        }
        
        [HttpPost]

        public ActionResult CreateAluno(Aluno aluno)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "INSERT INTO Aluno (Nome_Aluno,Sobrenome_Aluno,RM_Aluno,Telefone_Aluno,Curso,Status_Aluno) VALUES (@Nome,@Sobrenome,@RM,@Telefone,@Curso,@Status)";
                SqlCommand comand = new SqlCommand(query, conection);
                comand.Parameters.AddWithValue("@Nome", aluno.Nome);
                comand.Parameters.AddWithValue("@Sobrenome", aluno.Sobrenome);
                comand.Parameters.AddWithValue("@RM", aluno.RM);
                comand.Parameters.AddWithValue("@Telefone", aluno.Telefone);
                comand.Parameters.AddWithValue("@Curso", aluno.Curso);
                comand.Parameters.AddWithValue("@Status", aluno.Status);
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

        public ActionResult UpdateAluno(int id, [FromBody] Aluno aluno)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                string query = "UPDATE Aluno SET Nome_Aluno = @Nome, Sobrenome_Aluno = @Sobrenome,RM_Aluno = @RM,Telefone_Aluno = @Telefone,Curso = @Curso,Status_Aluno = @Status WHERE Id_Aluno = @Id";
                SqlCommand comand = new SqlCommand(query, connection);
                comand.Parameters.AddWithValue("@Nome", aluno.Nome);
                comand.Parameters.AddWithValue("@Sobrenome", aluno.Sobrenome);
                comand.Parameters.AddWithValue("@RM", aluno.RM);
                comand.Parameters.AddWithValue("@Telefone", aluno.Telefone);
                comand.Parameters.AddWithValue("@Curso", aluno.Curso);
                comand.Parameters.AddWithValue("@Status", aluno.Status);
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
        [HttpDelete("{RM}")]
        [HttpDelete]
        public ActionResult DeleteAluno(int rm)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                string query = "DELETE FROM Aluno WHERE RM_Aluno = @RM";
                SqlCommand comand = new SqlCommand(query, connection);
                comand.Parameters.AddWithValue("@RM", rm);
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

