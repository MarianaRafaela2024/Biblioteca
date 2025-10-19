using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<bool> ValidarLogin([FromBody] Login login)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = "SELECT * FROM Login WHERE RM = @RM AND Senha = @Senha";
                SqlCommand command = new SqlCommand(query, conection);
                command.Parameters.AddWithValue("@RM", login.RM);
                command.Parameters.AddWithValue("@Senha", login.Senha);
                conection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return Ok(true);
                }
            }

            return BadRequest(false);
        }
    }
}