using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
                        ISBN = reader["ISBN"]?.ToString() ?? string.Empty,
                        Cond_Encardenacao = reader["Condicoes_Encadernacao"]?.ToString() ?? string.Empty,
                        Agen_Catalogadora = reader["Agencia_Catalogadora"]?.ToString() ?? string.Empty,
                        Idi_Catalogacao = reader["Idioma_Catalogacao"]?.ToString() ?? string.Empty,
                        Agen_Transcricao = reader["Agencia_Transcricao"]?.ToString() ?? string.Empty,
                        Agen_Modificacao = reader["Agencia_Modificacao"]?.ToString() ?? string.Empty,
                        Idi_Texto = reader["Idioma_Texto"]?.ToString() ?? string.Empty,
                        Idi_Resumo = reader["Idioma_Resumo"]?.ToString() ?? string.Empty,
                        Idi_Legenda = reader["Idioma_Legenda"]?.ToString() ?? string.Empty,
                        Numero_CDD = reader["Numero_CDD"]?.ToString() ?? string.Empty,
                        Numero_Item_CDD = reader["Numero_Item_CDD"]?.ToString() ?? string.Empty,
                        Num_Cham_Local = reader["Numero_Chamada_Local"]?.ToString() ?? string.Empty,
                        Num_Item_Local = reader["Numero_Item_Local"]?.ToString() ?? string.Empty,
                        Num_Cham_Secundaria = reader["Numero_Chamada_Secundaria"]?.ToString() ?? string.Empty,
                        Nome = reader["Nome_Livro"]?.ToString() ?? string.Empty,
                        Subtitulo = reader["Subtitulo"]?.ToString() ?? string.Empty,
                        Indi_Responsabilidade = reader["Indicacao_Responsabilidade"]?.ToString() ?? string.Empty,
                        Indi_Arti_Inicial = reader["Indicador_Artigo_Inicial"]?.ToString() ?? string.Empty,
                        Num_Edicao = reader["Numero_Edicao"]?.ToString() ?? string.Empty,
                        Mencao_Responsa_Edicao = reader["Mencao_Responsabilidade_Edicao"]?.ToString() ?? string.Empty,
                        Local_Publicacao = reader["Local_Publicacao"]?.ToString() ?? string.Empty,
                        Editora = reader["Editora"]?.ToString() ?? string.Empty,
                        Ano_Publicacao = reader["Ano_Publicacao"] != DBNull.Value ? Convert.ToInt32(reader["Ano_Publicacao"]) : 0,
                        Paginas = reader["Paginas"]?.ToString() ?? string.Empty,
                        Ilustracoes = reader["Ilustracoes"]?.ToString() ?? string.Empty,
                        Dimensoes = reader["Dimensoes"]?.ToString() ?? string.Empty,
                        Material_Adicional = reader["Material_Adicional"]?.ToString() ?? string.Empty,
                        Titulo_Serie = reader["Titulo_Serie"]?.ToString() ?? string.Empty,
                        Num_Serie = reader["Numero_Serie"]?.ToString() ?? string.Empty,
                        Notas_Gerais = reader["Notas_Gerais"]?.ToString() ?? string.Empty,
                        Nome_Pess_Assunto = reader["Nome_Pessoal_Assunto"]?.ToString() ?? string.Empty,
                        Datas_Pessoais = reader["Datas_Pessoal"]?.ToString() ?? string.Empty,
                        Funcao_Pessoal = reader["Funcao_Pessoal"]?.ToString() ?? string.Empty,
                        Topico = reader["Topico_Pessoal"]?.ToString() ?? string.Empty,
                        Titulo_Uniforme = reader["Titulo_Uniforme"]?.ToString() ?? string.Empty,
                        Forma_Uniforme = reader["Forma_Uniforme"]?.ToString() ?? string.Empty,
                        Periodo_Historico = reader["Periodo_Historico_Uniforme"]?.ToString() ?? string.Empty,
                        Local_Uniforme = reader["Localidade_Uniforme"]?.ToString() ?? string.Empty,
                        Assunto_Termo = reader["Assunto_Termo"]?.ToString() ?? string.Empty,
                        Forma_Termo = reader["Forma_Termo"]?.ToString() ?? string.Empty,
                        Periodo_Histo_Termo = reader["Periodo_Historico_Termo"]?.ToString() ?? string.Empty,
                        Local_Termo = reader["Localidade_Termo"]?.ToString() ?? string.Empty,
                        Info_Local = reader["Informacao_Local"]?.ToString() ?? string.Empty,
                        Status_Item = reader["Status_Item"]?.ToString() ?? string.Empty,
                        Status_Emprestimos = reader["Status_Emprestimo"]?.ToString() ?? string.Empty
                    };
                    Livros.Add(Livro);
                }
                reader.Close();
            }
            return Livros;
        }

        //[HttpGet("search/{searchTerm}")]
        //public ActionResult SearchLivro(string searchTerm)
        //{
        //    var Livros = new List<Livro>();

        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        connection.Open();

        //        bool isNumero = int.TryParse(searchTerm, out int rmInt);

        //        string query;
        //        SqlCommand command;

        //        if (isNumero)
        //        {
        //            query = "SELECT * FROM Livro WHERE RM_Livro = @RM";
        //            command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@RM", rmInt);
        //        }
        //        else
        //        {
        //            query = "SELECT * FROM Livro WHERE Nome_Livro LIKE @Nome";
        //            command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@Nome", "%" + searchTerm + "%");
        //        }

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                var Livro = new Livro
        //                {
        //                    Id = Convert.ToInt32(reader["Id_Livro"]),
        //                    Nome = reader["Nome_Livro"] as string ?? string.Empty,
        //                    Sobrenome = reader["Sobrenome_Livro"] as string ?? string.Empty,
        //                    RM = reader["RM_Livro"]?.ToString() ?? string.Empty,
        //                    Telefone = reader["Telefone_Livro"]?.ToString() ?? string.Empty,
        //                    Curso = reader["Curso"]?.ToString() ?? string.Empty,
        //                    Status = reader["Status_Livro"]?.ToString() ?? string.Empty
        //                };
        //                Livros.Add(Livro);
        //            }
        //        }
        //    }

        //    if (Livros.Count == 0)
        //        return NotFound();

        //    return Ok(Livros);
        //}


        //[HttpGet("{id}", Name = "GetLivroID")]

        //public ActionResult GetLivroId(int id)
        //{
        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        string query = "SELECT * FROM Livro WHERE ID = @Id";
        //        SqlCommand comand = new SqlCommand(query, connection);
        //        comand.Parameters.AddWithValue("@Id", id);
        //        connection.Open();

        //        SqlDataReader reader = comand.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            Livro Livro = new Livro()
        //            {
        //                Id = Convert.ToInt32(reader["Id_Livro"]),
        //                Nome = reader["Nome_Livro"].ToString(),
        //                Sobrenome = reader["Sobrenome_Livro"].ToString(),
        //                RM = reader["RM_Livro"]?.ToString() ?? string.Empty,
        //                Telefone = reader["Telefone_Livro"].ToString(),
        //                Curso = reader["Curso"].ToString(),
        //                Status = reader["Status_Livro"].ToString()
        //            };
        //            reader.Close();

        //            return Ok(Livro);
        //        }
        //        reader.Close();
        //    }
        //    return NotFound();
        //}


        [HttpGet("search")]
        public IActionResult Search(string? termo)
        {
            var livros = new List<object>();

            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                conection.Open();

                string sql = @"
        SELECT 
            l.Id_Livro, 
            l.Nome_Livro, 
            l.Subtitulo, 
            l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, 
            l.ISBN, 
            l.Assunto_Termo,
            STRING_AGG(a.Nome_Autor, ', ') AS Autores
        FROM Livro l
        LEFT JOIN Livro_Autor la ON l.Id_Livro = la.Id_Livro
        LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
        WHERE (@termo IS NULL OR
               l.Nome_Livro LIKE '%' + @termo + '%' OR
               l.Subtitulo LIKE '%' + @termo + '%' OR
               l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
               l.Assunto_Termo LIKE '%' + @termo + '%' OR
               CAST(l.Ano_Publicacao AS NVARCHAR) = @termo OR
               a.Nome_Autor LIKE '%' + @termo + '%'
        )
        GROUP BY 
            l.Id_Livro, l.Nome_Livro, l.Subtitulo, l.Indicacao_Responsabilidade, 
            l.Ano_Publicacao, l.ISBN, l.Assunto_Termo
        ORDER BY l.Nome_Livro";

                using (SqlCommand cmd = new SqlCommand(sql, conection))
                {
                    cmd.Parameters.AddWithValue("@termo", (object)termo ?? DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            livros.Add(new
                            {
                                Id_Livro = Convert.ToInt32(reader["Id_Livro"]),
                                Nome_Livro = reader["Nome_Livro"].ToString(),
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

            return Ok(livros);
        }





        [HttpPost]
        public ActionResult CreateLivro(Livro livro)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = @"INSERT INTO Livro (
                    ISBN,
                    Condicoes_Encadernacao,
                    Agencia_Catalogadora,
                    Idioma_Catalogacao,
                    Agencia_Transcricao,
                    Agencia_Modificacao,
                    Idioma_Texto,
                    Idioma_Resumo,
                    Idioma_Legenda,
                    Numero_CDD,
                    Numero_Item_CDD,
                    Numero_Chamada_Local,
                    Numero_Item_Local,
                    Numero_Chamada_Secundaria,
                    Nome_Livro,
                    Subtitulo,
                    Indicacao_Responsabilidade,
                    Indicador_Artigo_Inicial,
                    Numero_Edicao,
                    Mencao_Responsabilidade_Edicao,
                    Local_Publicacao,
                    Editora,
                    Ano_Publicacao,
                    Paginas,
                    Ilustracoes,
                    Dimensoes,
                    Material_Adicional,
                    Titulo_Serie,
                    Numero_Serie,
                    Notas_Gerais,
                    Nome_Pessoal_Assunto,
                    Datas_Pessoal,
                    Funcao_Pessoal,
                    Topico_Pessoal,
                    Titulo_Uniforme,
                    Forma_Uniforme,
                    Periodo_Historico_Uniforme,
                    Localidade_Uniforme,
                    Assunto_Termo,
                    Forma_Termo,
                    Periodo_Historico_Termo,
                    Localidade_Termo,
                    Informacao_Local,
                    Status_Item,
                    Status_Emprestimo
                ) VALUES (
                    @ISBN,
                    @Condicoes_Encadernacao,
                    @Agencia_Catalogadora,
                    @Idioma_Catalogacao,
                    @Agencia_Transcricao,
                    @Agencia_Modificacao,
                    @Idioma_Texto,
                    @Idioma_Resumo,
                    @Idioma_Legenda,
                    @Numero_CDD,
                    @Numero_Item_CDD,
                    @Numero_Chamada_Local,
                    @Numero_Item_Local,
                    @Numero_Chamada_Secundaria,
                    @Nome_Livro,
                    @Subtitulo,
                    @Indicacao_Responsabilidade,
                    @Indicador_Artigo_Inicial,
                    @Numero_Edicao,
                    @Mencao_Responsabilidade_Edicao,
                    @Local_Publicacao,
                    @Editora,
                    @Ano_Publicacao,
                    @Paginas,
                    @Ilustracoes,
                    @Dimensoes,
                    @Material_Adicional,
                    @Titulo_Serie,
                    @Numero_Serie,
                    @Notas_Gerais,
                    @Nome_Pessoal_Assunto,
                    @Datas_Pessoal,
                    @Funcao_Pessoal,
                    @Topico_Pessoal,
                    @Titulo_Uniforme,
                    @Forma_Uniforme,
                    @Periodo_Historico_Uniforme,
                    @Localidade_Uniforme,
                    @Assunto_Termo,
                    @Forma_Termo,
                    @Periodo_Historico_Termo,
                    @Localidade_Termo,
                    @Informacao_Local,
                    @Status_Item,
                    @Status_Emprestimo
                )";

                SqlCommand comand = new SqlCommand(query, conection);
                comand.Parameters.AddWithValue("@ISBN", livro.ISBN);
                comand.Parameters.AddWithValue("@Condicoes_Encadernacao", livro.Cond_Encardenacao);
                comand.Parameters.AddWithValue("@Agencia_Catalogadora", livro.Agen_Catalogadora);
                comand.Parameters.AddWithValue("@Idioma_Catalogacao", livro.Idi_Catalogacao);
                comand.Parameters.AddWithValue("@Agencia_Transcricao", livro.Agen_Transcricao);
                comand.Parameters.AddWithValue("@Agencia_Modificacao", livro.Agen_Modificacao);
                comand.Parameters.AddWithValue("@Idioma_Texto", livro.Idi_Texto);
                comand.Parameters.AddWithValue("@Idioma_Resumo", livro.Idi_Resumo);
                comand.Parameters.AddWithValue("@Idioma_Legenda", livro.Idi_Legenda);
                comand.Parameters.AddWithValue("@Numero_CDD", livro.Numero_CDD);
                comand.Parameters.AddWithValue("@Numero_Item_CDD", livro.Numero_Item_CDD);
                comand.Parameters.AddWithValue("@Numero_Chamada_Local", livro.Num_Cham_Local);
                comand.Parameters.AddWithValue("@Numero_Item_Local", livro.Num_Item_Local);
                comand.Parameters.AddWithValue("@Numero_Chamada_Secundaria", livro.Num_Cham_Secundaria);
                comand.Parameters.AddWithValue("@Nome_Livro", livro.Nome);
                comand.Parameters.AddWithValue("@Subtitulo", livro.Subtitulo);
                comand.Parameters.AddWithValue("@Indicacao_Responsabilidade", livro.Indi_Responsabilidade);
                comand.Parameters.AddWithValue("@Indicador_Artigo_Inicial", livro.Indi_Arti_Inicial);
                comand.Parameters.AddWithValue("@Numero_Edicao", livro.Num_Edicao);
                comand.Parameters.AddWithValue("@Mencao_Responsabilidade_Edicao", livro.Mencao_Responsa_Edicao);
                comand.Parameters.AddWithValue("@Local_Publicacao", livro.Local_Publicacao);
                comand.Parameters.AddWithValue("@Editora", livro.Editora);
                comand.Parameters.AddWithValue("@Ano_Publicacao", livro.Ano_Publicacao);
                comand.Parameters.AddWithValue("@Paginas", livro.Paginas);
                comand.Parameters.AddWithValue("@Ilustracoes", livro.Ilustracoes);
                comand.Parameters.AddWithValue("@Dimensoes", livro.Dimensoes);
                comand.Parameters.AddWithValue("@Material_Adicional", livro.Material_Adicional);
                comand.Parameters.AddWithValue("@Titulo_Serie", livro.Titulo_Serie);
                comand.Parameters.AddWithValue("@Numero_Serie", livro.Num_Serie);
                comand.Parameters.AddWithValue("@Notas_Gerais", livro.Notas_Gerais);
                comand.Parameters.AddWithValue("@Nome_Pessoal_Assunto", livro.Nome_Pess_Assunto);
                comand.Parameters.AddWithValue("@Datas_Pessoal", livro.Datas_Pessoais);
                comand.Parameters.AddWithValue("@Funcao_Pessoal", livro.Funcao_Pessoal);
                comand.Parameters.AddWithValue("@Topico_Pessoal", livro.Topico);
                comand.Parameters.AddWithValue("@Titulo_Uniforme", livro.Titulo_Uniforme);
                comand.Parameters.AddWithValue("@Forma_Uniforme", livro.Forma_Uniforme);
                comand.Parameters.AddWithValue("@Periodo_Historico_Uniforme", livro.Periodo_Historico);
                comand.Parameters.AddWithValue("@Forma_Uniforme", livro.Forma_Uniforme);
                comand.Parameters.AddWithValue("@Periodo_Historico_Uniforme", livro.Periodo_Historico);
                comand.Parameters.AddWithValue("@Localidade_Uniforme", livro.Local_Uniforme);
                comand.Parameters.AddWithValue("@Assunto_Termo", livro.Assunto_Termo);
                comand.Parameters.AddWithValue("@Forma_Termo", livro.Forma_Termo);
                comand.Parameters.AddWithValue("@Periodo_Historico_Termo", livro.Periodo_Histo_Termo);
                comand.Parameters.AddWithValue("@Localidade_Termo", livro.Local_Termo);
                comand.Parameters.AddWithValue("@Informacao_Local", livro.Info_Local);
                comand.Parameters.AddWithValue("@Status_Item", livro.Status_Item);
                comand.Parameters.AddWithValue("@Status_Emprestimo", livro.Status_Emprestimos);

                conection.Open();
                int rowsAffected = comand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    

    [HttpPost("PostLivroLeigos")]
        public ActionResult CreateLivroLeigos(Livro livro)
        {
            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = @"INSERT INTO Livro (
                    Nome_Livro,
                    Editora,
                    Ano_Publicacao,
                    Paginas,
                    Notas_Gerais,
                    Assunto_Termo,
                    Status_Emprestimo
                ) VALUES (
                    @Nome_Livro,
                    @Editora,
                    @Ano_Publicacao,
                    @Paginas,
                    @Notas_Gerais,
                    @Assunto_Termo,
                    @Status_Emprestimo
                )";

                SqlCommand comand = new SqlCommand(query, conection);
                comand.Parameters.AddWithValue("@Nome_Livro", livro.Nome);
                comand.Parameters.AddWithValue("@Editora", livro.Editora);
                comand.Parameters.AddWithValue("@Ano_Publicacao", livro.Ano_Publicacao);
                comand.Parameters.AddWithValue("@Paginas", livro.Paginas);
                comand.Parameters.AddWithValue("@Notas_Gerais", livro.Notas_Gerais);
                comand.Parameters.AddWithValue("@Assunto_Termo", livro.Assunto_Termo);
                comand.Parameters.AddWithValue("@Status_Emprestimo", livro.Status_Emprestimos);

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

    //public ActionResult UpdateLivro(int id, [FromBody] Livro Livro)
    //{
    //    using (SqlConnection connection = new SqlConnection(StrConex))
    //    {
    //        string query = "UPDATE Livro SET Nome_Livro = @Nome, Sobrenome_Livro = @Sobrenome,RM_Livro = @RM,Telefone_Livro = @Telefone,Curso = @Curso,Status_Livro = @Status WHERE Id_Livro = @Id";
    //        SqlCommand comand = new SqlCommand(query, connection);
    //        comand.Parameters.AddWithValue("@Nome", Livro.Nome);
    //        comand.Parameters.AddWithValue("@Sobrenome", Livro.Sobrenome);
    //        comand.Parameters.AddWithValue("@RM", Livro.RM);
    //        comand.Parameters.AddWithValue("@Telefone", Livro.Telefone);
    //        comand.Parameters.AddWithValue("@Curso", Livro.Curso);
    //        comand.Parameters.AddWithValue("@Status", Livro.Status);
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


