using Biblioteca;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("Emprestimo")]
    public class EmprestimoController : Controller
    {
        private readonly ILogger<EmprestimoController> _logger;
        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public EmprestimoController(ILogger<EmprestimoController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult CreateEmprestimo(Emprestimo emprestimo)
        {
            if (emprestimo == null)
            {
                return BadRequest("Dados do empréstimo são obrigatórios.");
            }

            if (string.IsNullOrEmpty(emprestimo.Rm_Aluno))
            {
                return BadRequest("RM do aluno é obrigatório.");
            }

            if (emprestimo.Id_Livro <= 0)
            {
                return BadRequest("ID do livro é obrigatório.");
            }

            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                try
                {
                    connection.Open();

                    //valida se aluno existe
                    string checkAluno = "SELECT COUNT(*) FROM Aluno WHERE RM_Aluno = @RM_Aluno";
                    SqlCommand checkAlunoCmd = new SqlCommand(checkAluno, connection);
                    checkAlunoCmd.Parameters.AddWithValue("@RM_Aluno", emprestimo.Rm_Aluno);
                    int alunoExists = (int)checkAlunoCmd.ExecuteScalar();

                    if (alunoExists == 0)
                    {
                        return BadRequest($"Aluno com RM {emprestimo.Rm_Aluno} não encontrado.");
                    }

                    //valida se livro existe
                    string checkLivro = "SELECT COUNT(*) FROM Livro WHERE Id_Livro = @Id_Livro";
                    SqlCommand checkLivroCmd = new SqlCommand(checkLivro, connection);
                    checkLivroCmd.Parameters.AddWithValue("@Id_Livro", emprestimo.Id_Livro);
                    int livroExists = (int)checkLivroCmd.ExecuteScalar();

                    if (livroExists == 0)
                    {
                        return BadRequest($"Livro com ID {emprestimo.Id_Livro} não encontrado.");
                    }

                    string query = "INSERT INTO Emprestimo (Rm_Aluno, Id_Livro, Data_Emprestimo, Data_Devolucao_Prevista, Data_Devolucao_Real) VALUES (@Rm_Aluno, @Id_Livro, @Data_Emprestimo, @Data_Dev_Prev, @Data_Dev_Real)";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Rm_Aluno", emprestimo.Rm_Aluno);
                    command.Parameters.AddWithValue("@Id_Livro", emprestimo.Id_Livro);
                    command.Parameters.AddWithValue("@Data_Emprestimo", emprestimo.Data_Emprestimo.Date);
                    command.Parameters.AddWithValue("@Data_Dev_Prev", emprestimo.Data_Devolucao_Prevista.Date);
                    command.Parameters.AddWithValue("@Data_Dev_Real",
                        emprestimo.Data_Devolucao_Real.HasValue
                            ? emprestimo.Data_Devolucao_Real.Value.Date
                            : (object)DBNull.Value);

                    string updateStatus = "UPDATE Livro SET Status_Emprestimo = 'Emprestado' WHERE Id_Livro = @Id_Livro";
                    SqlCommand commandUp = new SqlCommand(updateStatus, connection);
                    commandUp.Parameters.AddWithValue("@Id_Livro", emprestimo.Id_Livro);


                    int rowsAffected = command.ExecuteNonQuery();
                    int rowsAffectedUp = commandUp.ExecuteNonQuery();

                    if (rowsAffected > 0 && rowsAffectedUp>0)
                    {
                        return Ok("Empréstimo criado com sucesso!");
                    }
                    return BadRequest("Nenhuma linha foi inserida.");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro: {ex.Message}");
                }
            }
        }



        [HttpGet]
        public ActionResult GetEmprestimo()
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                string query = @"
                    SELECT 
                        e.Id_Emprestimo,
                        e.RM_Aluno,
                        a.Nome_Aluno + ' ' + a.Sobrenome_Aluno AS NomeCompleto,
                        e.Id_Livro,
                        l.Nome_Livro,
                        e.Data_Emprestimo,
                        e.Data_Devolucao_Prevista,
                        e.Data_Devolucao_Real
                    FROM Emprestimo e
                    INNER JOIN Aluno a ON e.RM_Aluno = a.RM_Aluno
                    INNER JOIN Livro l ON e.Id_Livro = l.Id_Livro
                    ORDER BY e.Data_Emprestimo DESC";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    var emprestimo = new List<object>();

                    while (reader.Read())
                    {
                        emprestimo.Add(new
                        {
                            id_Emprestimo = reader["Id_Emprestimo"],
                            rm_Aluno = reader["Rm_Aluno"],
                            nomeAluno = reader["NomeCompleto"],
                            id_Livro = reader["Id_Livro"],
                            nomeLivro = reader["Nome_Livro"],
                            data_Emprestimo = reader["Data_Emprestimo"],
                            data_Devolucao_Prevista = reader["Data_Devolucao_Prevista"],
                            data_Devolucao_Real = reader["Data_Devolucao_Real"] != DBNull.Value
                                ? reader["Data_Devolucao_Real"]
                                : null
                        });
                    }

                    return Ok(emprestimo);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao buscar empréstimos");
                    return StatusCode(500, $"Erro: {ex.Message}");
                }
            }
        }

        [HttpPut("{id}/devolver")]
        public ActionResult RegistrarDevolucao(int id)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                try
                {
                    connection.Open();

                    // ✅ Primeiro: busca o Id_Livro e verifica se já foi devolvido
                    string checkQuery = "SELECT Id_Livro, Data_Devolucao_Real FROM Emprestimo WHERE Id_Emprestimo = @Id";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = checkCmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        reader.Close();
                        return NotFound($"Empréstimo com ID {id} não encontrado.");
                    }

                    int idLivro = Convert.ToInt32(reader["Id_Livro"]);
                    var dataDevolucaoReal = reader["Data_Devolucao_Real"];

                    reader.Close(); // ✅ IMPORTANTE: Fecha o reader antes de executar outros comandos

                    if (dataDevolucaoReal != DBNull.Value)
                    {
                        return BadRequest("Este empréstimo já foi devolvido.");
                    }

                    // ✅ Segundo: atualiza a data de devolução do empréstimo
                    string updateQuery = "UPDATE Emprestimo SET Data_Devolucao_Real = @DataDevolucao WHERE Id_Emprestimo = @Id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Id", id);
                    updateCmd.Parameters.AddWithValue("@DataDevolucao", DateTime.Now.Date);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    // ✅ Terceiro: atualiza o status do livro
                    string updateStatus = "UPDATE Livro SET Status_Emprestimo = 'Devolvido' WHERE Id_Livro = @Id_Livro";
                    SqlCommand commandUp = new SqlCommand(updateStatus, connection);
                    commandUp.Parameters.AddWithValue("@Id_Livro", idLivro);

                    int rowsAffectedUp = commandUp.ExecuteNonQuery();

                    if (rowsAffected > 0 && rowsAffectedUp > 0)
                    {
                        return Ok(new { message = "Devolução registrada com sucesso!" });
                    }

                    return BadRequest("Erro ao registrar devolução.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao registrar devolução");
                    return StatusCode(500, $"Erro: {ex.Message}");
                }
            }
        }

        [HttpDelete]
        public ActionResult DeleteEmprestimo(int id)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                connection.Open();
                SqlCommand comand = new SqlCommand("DELETE FROM Emprestimo WHERE Id_Emprestimo = @Id_Emprestimo", connection);
                comand.Parameters.AddWithValue("@Id_Emprestimo", id);

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

