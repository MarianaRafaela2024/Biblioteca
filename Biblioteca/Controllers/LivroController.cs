using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Windows.Input;

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
                        Status_Emprestimos = reader["Status_Emprestimos"]?.ToString() ?? string.Empty
                    };
                    Livros.Add(Livro);
                }

                return Livros;
            }
        }


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
                            l.Status_Emprestimos,
                            STRING_AGG(a.Nome_Autor, ', ') AS Autores
                        FROM Livro l
                        LEFT JOIN Livro_Autor la ON l.Id_Livro = la.Id_Livro
                        LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
                        WHERE (@termo IS NULL OR
                            l.Nome_Livro LIKE '%' + @termo + '%' OR
                            l.Subtitulo LIKE '%' + @termo + '%' OR
                            l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
                            l.Assunto_Termo LIKE '%' + @termo + '%' OR
                            l.Status_Emprestimos LIKE '%' + @termo + '%' OR
                            CAST(l.Ano_Publicacao AS NVARCHAR) = @termo OR
                            a.Nome_Autor LIKE '%' + @termo + '%'
                        )
                        GROUP BY 
                            l.Id_Livro, l.Nome_Livro, l.Subtitulo, l.Indicacao_Responsabilidade,
                            l.Ano_Publicacao, l.ISBN, l.Assunto_Termo, l.Status_Emprestimos
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
                                Autores = reader["Autores"]?.ToString(),
                                Status_Emprestimos = reader["Status_Emprestimos"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(livros);
        }



        //[HttpPost]
        //public ActionResult CreateLivro(BibliotecaRequest CreateL)
        //{
        //    using (SqlConnection conection = new SqlConnection(StrConex))
        //    {
        //        string query = @"INSERT INTO Livro (
        //            ISBN,
        //            Condicoes_Encadernacao,
        //            Agencia_Catalogadora,
        //            Idioma_Catalogacao,
        //            Agencia_Transcricao,
        //            Agencia_Modificacao,
        //            Idioma_Texto,
        //            Idioma_Resumo,
        //            Idioma_Legenda,
        //            Numero_CDD,
        //            Numero_Item_CDD,
        //            Numero_Chamada_Local,
        //            Numero_Item_Local,
        //            Numero_Chamada_Secundaria,
        //            Nome_Livro,
        //            Subtitulo,
        //            Indicacao_Responsabilidade,
        //            Indicador_Artigo_Inicial,
        //            Numero_Edicao,
        //            Mencao_Responsabilidade_Edicao,
        //            Local_Publicacao,
        //            Editora,
        //            Ano_Publicacao,
        //            Paginas,
        //            Ilustracoes,
        //            Dimensoes,
        //            Material_Adicional,
        //            Titulo_Serie,
        //            Numero_Serie,
        //            Notas_Gerais,
        //            Nome_Pessoal_Assunto,
        //            Datas_Pessoal,
        //            Funcao_Pessoal,
        //            Topico_Pessoal,
        //            Titulo_Uniforme,
        //            Forma_Uniforme,
        //            Periodo_Historico_Uniforme,
        //            Localidade_Uniforme,
        //            Assunto_Termo,
        //            Forma_Termo,
        //            Periodo_Historico_Termo,
        //            Localidade_Termo,
        //            Informacao_Local,
        //            Status_Item,
        //            Status_Emprestimo
        //        ) VALUES (
        //            @ISBN,
        //            @Condicoes_Encadernacao,
        //            @Agencia_Catalogadora,
        //            @Idioma_Catalogacao,
        //            @Agencia_Transcricao,
        //            @Agencia_Modificacao,
        //            @Idioma_Texto,
        //            @Idioma_Resumo,
        //            @Idioma_Legenda,
        //            @Numero_CDD,
        //            @Numero_Item_CDD,
        //            @Numero_Chamada_Local,
        //            @Numero_Item_Local,
        //            @Numero_Chamada_Secundaria,
        //            @Nome_Livro,
        //            @Subtitulo,
        //            @Indicacao_Responsabilidade,
        //            @Indicador_Artigo_Inicial,
        //            @Numero_Edicao,
        //            @Mencao_Responsabilidade_Edicao,
        //            @Local_Publicacao,
        //            @Editora,
        //            @Ano_Publicacao,
        //            @Paginas,
        //            @Ilustracoes,
        //            @Dimensoes,
        //            @Material_Adicional,
        //            @Titulo_Serie,
        //            @Numero_Serie,
        //            @Notas_Gerais,
        //            @Nome_Pessoal_Assunto,
        //            @Datas_Pessoal,
        //            @Funcao_Pessoal,
        //            @Topico_Pessoal,
        //            @Titulo_Uniforme,
        //            @Forma_Uniforme,
        //            @Periodo_Historico_Uniforme,
        //            @Localidade_Uniforme,
        //            @Assunto_Termo,
        //            @Forma_Termo,
        //            @Periodo_Historico_Termo,
        //            @Localidade_Termo,
        //            @Informacao_Local,
        //            @Status_Item,
        //            @Status_Emprestimo
        //        )

        //        INSERT INTO Autor (Nome_Autor,Numero,Datas,Funcao,Tipo_Autor) VALUES (@Nome_Autor,@Numero,@Datas,@Funcao,@Tipo_Autor)

        //        INSERT INTO Entidade_Corporativa (Nome_Entidade,Subordinacao) VALUES (@Nome_Entidade,@Subordinacao)";

        //        SqlCommand comand = new SqlCommand(query, conection);
        //        comand.Parameters.AddWithValue("@ISBN", CreateL.livro.ISBN);
        //        comand.Parameters.AddWithValue("@Condicoes_Encadernacao", CreateL.livro.Cond_Encardenacao);
        //        comand.Parameters.AddWithValue("@Agencia_Catalogadora", CreateL.livro.Agen_Catalogadora);
        //        comand.Parameters.AddWithValue("@Idioma_Catalogacao", CreateL.livro.Idi_Catalogacao);
        //        comand.Parameters.AddWithValue("@Agencia_Transcricao", CreateL.livro.Agen_Transcricao);
        //        comand.Parameters.AddWithValue("@Agencia_Modificacao", CreateL.livro.Agen_Modificacao);
        //        comand.Parameters.AddWithValue("@Idioma_Texto", CreateL.livro.Idi_Texto);
        //        comand.Parameters.AddWithValue("@Idioma_Resumo", CreateL.livro.Idi_Resumo);
        //        comand.Parameters.AddWithValue("@Idioma_Legenda", CreateL.livro.Idi_Legenda);
        //        comand.Parameters.AddWithValue("@Numero_CDD", CreateL.livro.Numero_CDD);
        //        comand.Parameters.AddWithValue("@Numero_Item_CDD", CreateL.livro.Numero_Item_CDD);
        //        comand.Parameters.AddWithValue("@Numero_Chamada_Local", CreateL.livro.Num_Cham_Local);
        //        comand.Parameters.AddWithValue("@Numero_Item_Local", CreateL.livro.Num_Item_Local);
        //        comand.Parameters.AddWithValue("@Numero_Chamada_Secundaria", CreateL.livro.Num_Cham_Secundaria);
        //        comand.Parameters.AddWithValue("@Nome_Livro", CreateL.livro.Nome);
        //        comand.Parameters.AddWithValue("@Subtitulo", CreateL.livro.Subtitulo);
        //        comand.Parameters.AddWithValue("@Indicacao_Responsabilidade", CreateL.livro.Indi_Responsabilidade);
        //        comand.Parameters.AddWithValue("@Indicador_Artigo_Inicial", CreateL.livro.Indi_Arti_Inicial);
        //        comand.Parameters.AddWithValue("@Numero_Edicao", CreateL.livro.Num_Edicao);
        //        comand.Parameters.AddWithValue("@Mencao_Responsabilidade_Edicao", CreateL.livro.Mencao_Responsa_Edicao);
        //        comand.Parameters.AddWithValue("@Local_Publicacao", CreateL.livro.Local_Publicacao);
        //        comand.Parameters.AddWithValue("@Editora", CreateL.livro.Editora);
        //        comand.Parameters.AddWithValue("@Ano_Publicacao", CreateL.livro.Ano_Publicacao);
        //        comand.Parameters.AddWithValue("@Paginas", CreateL.livro.Paginas);
        //        comand.Parameters.AddWithValue("@Ilustracoes", CreateL.livro.Ilustracoes);
        //        comand.Parameters.AddWithValue("@Dimensoes", CreateL.livro.Dimensoes);
        //        comand.Parameters.AddWithValue("@Material_Adicional", CreateL.livro.Material_Adicional);
        //        comand.Parameters.AddWithValue("@Titulo_Serie", CreateL.livro.Titulo_Serie);
        //        comand.Parameters.AddWithValue("@Numero_Serie", CreateL.livro.Num_Serie);
        //        comand.Parameters.AddWithValue("@Notas_Gerais", CreateL.livro.Notas_Gerais);
        //        comand.Parameters.AddWithValue("@Nome_Pessoal_Assunto", CreateL.livro.Nome_Pess_Assunto);
        //        comand.Parameters.AddWithValue("@Datas_Pessoal", CreateL.livro.Datas_Pessoais);
        //        comand.Parameters.AddWithValue("@Funcao_Pessoal", CreateL.livro.Funcao_Pessoal);
        //        comand.Parameters.AddWithValue("@Topico_Pessoal", CreateL.livro.Topico);
        //        comand.Parameters.AddWithValue("@Titulo_Uniforme", CreateL.livro.Titulo_Uniforme);
        //        comand.Parameters.AddWithValue("@Forma_Uniforme", CreateL.livro.Forma_Uniforme);
        //        comand.Parameters.AddWithValue("@Periodo_Historico_Uniforme", CreateL.livro.Periodo_Historico);
        //        comand.Parameters.AddWithValue("@Localidade_Uniforme", CreateL.livro.Local_Uniforme);
        //        comand.Parameters.AddWithValue("@Assunto_Termo", CreateL.livro.Assunto_Termo);
        //        comand.Parameters.AddWithValue("@Forma_Termo", CreateL.livro.Forma_Termo);
        //        comand.Parameters.AddWithValue("@Periodo_Historico_Termo", CreateL.livro.Periodo_Histo_Termo);
        //        comand.Parameters.AddWithValue("@Localidade_Termo", CreateL.livro.Local_Termo);
        //        comand.Parameters.AddWithValue("@Informacao_Local", CreateL.livro.Info_Local);
        //        comand.Parameters.AddWithValue("@Status_Item", CreateL.livro.Status_Item);
        //        comand.Parameters.AddWithValue("@Status_Emprestimo", CreateL.livro.Status_Emprestimos);
        //        Autor
        //            comand.Parameters.AddWithValue("@Nome_Autor", CreateL.autor.Nome_Autor);
        //        comand.Parameters.AddWithValue("@Numero", CreateL.autor.Numero);
        //        comand.Parameters.AddWithValue("@Datas", CreateL.autor.Datas);
        //        comand.Parameters.AddWithValue("@Funcao", CreateL.autor.Funcao);
        //        comand.Parameters.AddWithValue("@Tipo_Autor", CreateL.autor.Tipo_Autor);
        //        Entidade_Corporativa
        //            comand.Parameters.AddWithValue("@Nome_Entidade", CreateL.entidade.Nome_Entidade);
        //        comand.Parameters.AddWithValue("@Subordinacao", CreateL.entidade.Subordinacao);

        //        conection.Open();
        //        int rowsAffected = comand.ExecuteNonQuery();

        //        if (rowsAffected > 0)
        //        {
        //            return Ok();
        //        }
        //    }
        //    return BadRequest();
        //}

        [HttpPost]
        public ActionResult CreateLivro(BibliotecaRequest CreateL)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int autorId = 0;
                    int entidadeId = 0;
                    int livroId = 0;

                    // ========== VERIFICAR E INSERIR AUTOR ==========
                    if (!string.IsNullOrEmpty(CreateL.autor?.Nome_Autor))
                    {
                        string checkAutorQuery = "SELECT Id_Autor FROM Autor WHERE Nome_Autor = @Nome_Autor";
                        using (SqlCommand checkCmd = new SqlCommand(checkAutorQuery, connection, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@Nome_Autor", CreateL.autor.Nome_Autor);
                            var result = checkCmd.ExecuteScalar();

                            if (result != null)
                            {
                                autorId = Convert.ToInt32(result);
                            }
                            else
                            {
                                string insertAutorQuery = @"
                            INSERT INTO Autor (Nome_Autor, Numero, Datas, Funcao, Tipo_Autor) 
                            VALUES (@Nome_Autor, @Numero, @Datas, @Funcao, @Tipo_Autor);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                using (SqlCommand insertCmd = new SqlCommand(insertAutorQuery, connection, transaction))
                                {
                                    insertCmd.Parameters.AddWithValue("@Nome_Autor", CreateL.autor.Nome_Autor);
                                    insertCmd.Parameters.AddWithValue("@Numero", CreateL.autor.Numero ?? (object)DBNull.Value);
                                    insertCmd.Parameters.AddWithValue("@Datas", CreateL.autor.Datas ?? (object)DBNull.Value);
                                    insertCmd.Parameters.AddWithValue("@Funcao", CreateL.autor.Funcao ?? (object)DBNull.Value);
                                    insertCmd.Parameters.AddWithValue("@Tipo_Autor", CreateL.autor.Tipo_Autor ?? (object)DBNull.Value);

                                    autorId = (int)insertCmd.ExecuteScalar();
                                }
                            }
                        }
                    }

                    // ========== VERIFICAR E INSERIR ENTIDADE ==========
                    if (!string.IsNullOrEmpty(CreateL.entidade?.Nome_Entidade))
                    {
                        string checkEntidadeQuery = "SELECT Id_Entidade FROM Entidade_Corporativa WHERE Nome_Entidade = @Nome_Entidade";
                        using (SqlCommand checkCmd = new SqlCommand(checkEntidadeQuery, connection, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@Nome_Entidade", CreateL.entidade.Nome_Entidade);
                            var result = checkCmd.ExecuteScalar();

                            if (result != null || CreateL.entidade.Nome_Entidade.ToString() == "string" || CreateL.entidade.Nome_Entidade.ToString() == null)
                            {
                                entidadeId = Convert.ToInt32(result);
                            }
                            else
                            {
                                string insertEntidadeQuery = @"
                            INSERT INTO Entidade_Corporativa (Nome_Entidade, Subordinacao) 
                            VALUES (@Nome_Entidade, @Subordinacao);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                using (SqlCommand insertCmd = new SqlCommand(insertEntidadeQuery, connection, transaction))
                                {
                                    insertCmd.Parameters.AddWithValue("@Nome_Entidade", CreateL.entidade.Nome_Entidade);
                                    insertCmd.Parameters.AddWithValue("@Subordinacao", CreateL.entidade.Subordinacao ?? (object)DBNull.Value);

                                    entidadeId = (int)insertCmd.ExecuteScalar();
                                }
                            }
                        }
                    }

                    // ========== VERIFICAR SE LIVRO JÁ EXISTE PELO ISBN ==========
                    if (!string.IsNullOrEmpty(CreateL.livro?.ISBN))
                    {
                        string checkLivroQuery = "SELECT Id_Livro FROM Livro WHERE ISBN = @ISBN";
                        using (SqlCommand checkCmd = new SqlCommand(checkLivroQuery, connection, transaction))
                        {
                            checkCmd.Parameters.AddWithValue("@ISBN", CreateL.livro.ISBN);
                            var result = checkCmd.ExecuteScalar();

                            if (result != null)
                            {
                                livroId = Convert.ToInt32(result);
                                transaction.Rollback();
                                return BadRequest(new
                                {
                                    message = "Livro com este ISBN já existe!",
                                    livroId = livroId,
                                    isbn = CreateL.livro.ISBN
                                });
                            }
                        }
                    }

                    // ========== INSERIR LIVRO ==========
                    string insertLivroQuery = @"
                INSERT INTO Livro (
                    ISBN, Condicoes_Encadernacao, Agencia_Catalogadora, Idioma_Catalogacao,
                    Agencia_Transcricao, Agencia_Modificacao, Idioma_Texto, Idioma_Resumo,
                    Idioma_Legenda, Numero_CDD, Numero_Item_CDD, Numero_Chamada_Local,
                    Numero_Item_Local, Numero_Chamada_Secundaria, Nome_Livro, Subtitulo,
                    Indicacao_Responsabilidade, Indicador_Artigo_Inicial, Numero_Edicao,
                    Mencao_Responsabilidade_Edicao, Local_Publicacao, Editora, Ano_Publicacao,
                    Paginas, Ilustracoes, Dimensoes, Material_Adicional, Titulo_Serie,
                    Numero_Serie, Notas_Gerais, Nome_Pessoal_Assunto, Datas_Pessoal,
                    Funcao_Pessoal, Topico_Pessoal, Titulo_Uniforme, Forma_Uniforme,
                    Periodo_Historico_Uniforme, Localidade_Uniforme, Assunto_Termo,
                    Forma_Termo, Periodo_Historico_Termo, Localidade_Termo, Informacao_Local,
                    Status_Item, Status_Emprestimos
                ) VALUES (
                    @ISBN, @Condicoes_Encadernacao, @Agencia_Catalogadora, @Idioma_Catalogacao,
                    @Agencia_Transcricao, @Agencia_Modificacao, @Idioma_Texto, @Idioma_Resumo,
                    @Idioma_Legenda, @Numero_CDD, @Numero_Item_CDD, @Numero_Chamada_Local,
                    @Numero_Item_Local, @Numero_Chamada_Secundaria, @Nome_Livro, @Subtitulo,
                    @Indicacao_Responsabilidade, @Indicador_Artigo_Inicial, @Numero_Edicao,
                    @Mencao_Responsabilidade_Edicao, @Local_Publicacao, @Editora, @Ano_Publicacao,
                    @Paginas, @Ilustracoes, @Dimensoes, @Material_Adicional, @Titulo_Serie,
                    @Numero_Serie, @Notas_Gerais, @Nome_Pessoal_Assunto, @Datas_Pessoal,
                    @Funcao_Pessoal, @Topico_Pessoal, @Titulo_Uniforme, @Forma_Uniforme,
                    @Periodo_Historico_Uniforme, @Localidade_Uniforme, @Assunto_Termo,
                    @Forma_Termo, @Periodo_Historico_Termo, @Localidade_Termo, @Informacao_Local,
                    @Status_Item, @Status_Emprestimos
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand comandLA = new SqlCommand(insertLivroQuery, connection, transaction))
                    {
                        // Adicionar todos os parâmetros com verificação de NULL
                        comandLA.Parameters.AddWithValue("@ISBN", CreateL.livro.ISBN ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Condicoes_Encadernacao", CreateL.livro.Cond_Encardenacao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Agencia_Catalogadora", CreateL.livro.Agen_Catalogadora ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Idioma_Catalogacao", CreateL.livro.Idi_Catalogacao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Agencia_Transcricao", CreateL.livro.Agen_Transcricao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Agencia_Modificacao", CreateL.livro.Agen_Modificacao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Idioma_Texto", CreateL.livro.Idi_Texto ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Idioma_Resumo", CreateL.livro.Idi_Resumo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Idioma_Legenda", CreateL.livro.Idi_Legenda ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_CDD", CreateL.livro.Numero_CDD ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Item_CDD", CreateL.livro.Numero_Item_CDD ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Chamada_Local", CreateL.livro.Num_Cham_Local ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Item_Local", CreateL.livro.Num_Item_Local ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Chamada_Secundaria", CreateL.livro.Num_Cham_Secundaria ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Nome_Livro", CreateL.livro.Nome ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Subtitulo", CreateL.livro.Subtitulo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Indicacao_Responsabilidade", CreateL.livro.Indi_Responsabilidade ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Indicador_Artigo_Inicial", CreateL.livro.Indi_Arti_Inicial ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Edicao", CreateL.livro.Num_Edicao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Mencao_Responsabilidade_Edicao", CreateL.livro.Mencao_Responsa_Edicao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Local_Publicacao", CreateL.livro.Local_Publicacao ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Editora", CreateL.livro.Editora ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Ano_Publicacao", CreateL.livro.Ano_Publicacao > 0 ? CreateL.livro.Ano_Publicacao : (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Paginas", CreateL.livro.Paginas ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Ilustracoes", CreateL.livro.Ilustracoes ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Dimensoes", CreateL.livro.Dimensoes ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Material_Adicional", CreateL.livro.Material_Adicional ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Titulo_Serie", CreateL.livro.Titulo_Serie ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Numero_Serie", CreateL.livro.Num_Serie ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Notas_Gerais", CreateL.livro.Notas_Gerais ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Nome_Pessoal_Assunto", CreateL.livro.Nome_Pess_Assunto ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Datas_Pessoal", CreateL.livro.Datas_Pessoais ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Funcao_Pessoal", CreateL.livro.Funcao_Pessoal ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Topico_Pessoal", CreateL.livro.Topico ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Titulo_Uniforme", CreateL.livro.Titulo_Uniforme ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Forma_Uniforme", CreateL.livro.Forma_Uniforme ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Periodo_Historico_Uniforme", CreateL.livro.Periodo_Historico ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Localidade_Uniforme", CreateL.livro.Local_Uniforme ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Assunto_Termo", CreateL.livro.Assunto_Termo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Forma_Termo", CreateL.livro.Forma_Termo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Periodo_Historico_Termo", CreateL.livro.Periodo_Histo_Termo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Localidade_Termo", CreateL.livro.Local_Termo ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Informacao_Local", CreateL.livro.Info_Local ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Status_Item", CreateL.livro.Status_Item ?? (object)DBNull.Value);
                        comandLA.Parameters.AddWithValue("@Status_Emprestimos", CreateL.livro.Status_Emprestimos);

                        livroId = (int)comandLA.ExecuteScalar();
                    }

                    // ========== INSERIR RELACIONAMENTO LIVRO-AUTOR ==========
                    if (autorId > 0)
                    {
                        string livroAutorInsert = "INSERT INTO Livro_Autor (Id_Autor, Id_Livro) VALUES (@Id_Autor, @Id_Livro)";
                        using (SqlCommand comandLA = new SqlCommand(livroAutorInsert, connection, transaction))
                        {
                            comandLA.Parameters.AddWithValue("@Id_Autor", autorId);
                            comandLA.Parameters.AddWithValue("@Id_Livro", livroId);
                            comandLA.ExecuteNonQuery();
                        }
                    }

                    // ========== INSERIR RELACIONAMENTO LIVRO-ENTIDADE ==========
                    if (entidadeId > 0)
                    {
                        string livroEntidadeInsert = "INSERT INTO Livro_Entidade (Id_Entidade, Id_Livro) VALUES (@Id_Entidade, @Id_Livro)";
                        using (SqlCommand comandLE = new SqlCommand(livroEntidadeInsert, connection, transaction))
                        {
                            comandLE.Parameters.AddWithValue("@Id_Entidade", entidadeId);
                            comandLE.Parameters.AddWithValue("@Id_Livro", livroId);
                            comandLE.ExecuteNonQuery();
                        }
                    }
                    string exemplaresInsert = "INSERT INTO Exemplares_Automaticos (Id_Livro,Numero_Exemplares,Numero_Volume,Numero_Exemplares_Total,Data_Aquisicao,Biblioteca_Depositaria,Tipo_Aquisicao) VALUES (@Id_Livro,@Numero_Exemplares,@Numero_Volume,@Numero_Exemplares_Total,@Data_Aquisicao,@Biblioteca_Depositaria,@Tipo_Aquisicao)";
                    using (SqlCommand comandEx = new SqlCommand(exemplaresInsert, connection, transaction))
                    {
                        comandEx.Parameters.AddWithValue("@Id_Livro",livroId);
                        comandEx.Parameters.AddWithValue("@Numero_Exemplares", CreateL.Exemplares.Numero_Exemplares);
                        comandEx.Parameters.AddWithValue("@Numero_Volume", CreateL.Exemplares.Numero_Volume);
                        comandEx.Parameters.AddWithValue("@Numero_Exemplares_Total", CreateL.Exemplares.Numero_Exemplares_Total);
                        comandEx.Parameters.AddWithValue("@Data_Aquisicao", CreateL.Exemplares.Data_Aquisicao);
                        comandEx.Parameters.AddWithValue("@Biblioteca_Depositaria", CreateL.Exemplares.Biblioteca_Depositaria);
                        comandEx.Parameters.AddWithValue("@Tipo_Aquisicao", CreateL.Exemplares.Tipo_Aquisicao);
                    }
                    // Confirma a transação
                    transaction.Commit();
                    return Ok(new
                    {
                        message = "Livro cadastrado com sucesso!",
                        livroId = livroId,
                        autorId = autorId,
                        entidadeId = entidadeId
                    });
                }
                catch (Exception ex)
                {
                    // Desfaz todas as alterações
                    transaction.Rollback();
                    return BadRequest(new
                    {
                        message = "Erro ao cadastrar livro",
                        error = ex.Message,
                        stackTrace = ex.StackTrace
                    });
                }
            }
        }
        //[HttpPost]
        //public ActionResult CreateLivro(BibliotecaRequest CreateL)
        //{
        //    using (SqlConnection connection = new SqlConnection(StrConex))
        //    {
        //        connection.Open();
        //        SqlTransaction transaction = connection.BeginTransaction();

        //        try
        //        {
        //            int autorId = 0;
        //            int entidadeId = 0;
        //            int LivroId = 0;

        //            // ========== VERIFICAR E INSERIR AUTOR ==========
        //            if (!string.IsNullOrEmpty(CreateL.autor.Nome_Autor))
        //            {
        //                // Verificar se autor já existe
        //                string checkAutorQuery = "SELECT Id_Autor FROM Autor WHERE Nome_Autor = @Nome_Autor";
        //                using (SqlCommand checkCmd = new SqlCommand(checkAutorQuery, connection, transaction))
        //                {
        //                    checkCmd.Parameters.AddWithValue("@Nome_Autor", CreateL.autor.Nome_Autor);
        //                    var result = checkCmd.ExecuteScalar();

        //                    if (result != null)
        //                    {
        //                        // Autor já existe
        //                        autorId = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        // Inserir novo autor
        //                        string insertAutorQuery = @"
        //                    INSERT INTO Autor (Nome_Autor, Numero, Datas, Funcao, Tipo_Autor) 
        //                    VALUES (@Nome_Autor, @Numero, @Datas, @Funcao, @Tipo_Autor);
        //                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

        //                        using (SqlCommand insertCmd = new SqlCommand(insertAutorQuery, connection, transaction))
        //                        {
        //                            insertCmd.Parameters.AddWithValue("@Nome_Autor", CreateL.autor.Nome_Autor);
        //                            insertCmd.Parameters.AddWithValue("@Numero", CreateL.autor.Numero ?? (object)DBNull.Value);
        //                            insertCmd.Parameters.AddWithValue("@Datas", CreateL.autor.Datas ?? (object)DBNull.Value);
        //                            insertCmd.Parameters.AddWithValue("@Funcao", CreateL.autor.Funcao ?? (object)DBNull.Value);
        //                            insertCmd.Parameters.AddWithValue("@Tipo_Autor", CreateL.autor.Tipo_Autor ?? (object)DBNull.Value);

        //                            autorId = (int)insertCmd.ExecuteScalar();
        //                        }
        //                    }
        //                }
        //            }

        //            // ========== VERIFICAR E INSERIR ENTIDADE ==========
        //            if (!string.IsNullOrEmpty(CreateL.entidade.Nome_Entidade))
        //            {
        //                // Verificar se entidade já existe
        //                string checkEntidadeQuery = "SELECT Id_Entidade FROM Entidade_Corporativa WHERE Nome_Entidade = @Nome_Entidade";
        //                using (SqlCommand checkCmd = new SqlCommand(checkEntidadeQuery, connection, transaction))
        //                {
        //                    checkCmd.Parameters.AddWithValue("@Nome_Entidade", CreateL.entidade.Nome_Entidade);
        //                    var result = checkCmd.ExecuteScalar();

        //                    if (result != null)
        //                    {
        //                        // Entidade já existe
        //                        entidadeId = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        // Inserir nova entidade
        //                        string insertEntidadeQuery = @"
        //                    INSERT INTO Entidade_Corporativa (Nome_Entidade, Subordinacao) 
        //                    VALUES (@Nome_Entidade, @Subordinacao);
        //                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

        //                        using (SqlCommand insertCmd = new SqlCommand(insertEntidadeQuery, connection, transaction))
        //                        {
        //                            insertCmd.Parameters.AddWithValue("@Nome_Entidade", CreateL.entidade.Nome_Entidade);
        //                            insertCmd.Parameters.AddWithValue("@Subordinacao", CreateL.entidade.Subordinacao ?? (object)DBNull.Value);

        //                            entidadeId = (int)insertCmd.ExecuteScalar();
        //                        }
        //                    }
        //                }
        //            }
        //            if (!string.IsNullOrEmpty(CreateL.livro.ISBN))
        //            {
        //                // Verificar se livro já existe
        //                string checkLivroQuery = "SELECT ISBN FROM Livro WHERE ISBN = @ISBN";
        //                using (SqlCommand checkCmd = new SqlCommand(checkLivroQuery, connection, transaction))
        //                {
        //                    checkCmd.Parameters.AddWithValue("@ISBN", CreateL.livro.ISBN);
        //                    var result = checkCmd.ExecuteScalar();

        //                    if (result != null)
        //                    {
        //                        // livro já existe
        //                        LivroId = Convert.ToInt32(result);
        //                    }
        //                    else
        //                    {
        //                        // ========== INSERIR LIVRO ==========
        //                        string insertLivroQuery = @"INSERT INTO Livro (
        //                            ISBN,
        //                            Condicoes_Encadernacao,
        //                            Agencia_Catalogadora,
        //                            Idioma_Catalogacao,
        //                            Agencia_Transcricao,
        //                            Agencia_Modificacao,
        //                            Idioma_Texto,
        //                            Idioma_Resumo,
        //                            Idioma_Legenda,
        //                            Numero_CDD,
        //                            Numero_Item_CDD,
        //                            Numero_Chamada_Local,
        //                            Numero_Item_Local,
        //                            Numero_Chamada_Secundaria,
        //                            Nome_Livro,
        //                            Subtitulo,
        //                            Indicacao_Responsabilidade,
        //                            Indicador_Artigo_Inicial,
        //                            Numero_Edicao,
        //                            Mencao_Responsabilidade_Edicao,
        //                            Local_Publicacao,
        //                            Editora,
        //                            Ano_Publicacao,
        //                            Paginas,
        //                            Ilustracoes,
        //                            Dimensoes,
        //                            Material_Adicional,
        //                            Titulo_Serie,
        //                            Numero_Serie,
        //                            Notas_Gerais,
        //                            Nome_Pessoal_Assunto,
        //                            Datas_Pessoal,
        //                            Funcao_Pessoal,
        //                            Topico_Pessoal,
        //                            Titulo_Uniforme,
        //                            Forma_Uniforme,
        //                            Periodo_Historico_Uniforme,
        //                            Localidade_Uniforme,
        //                            Assunto_Termo,
        //                            Forma_Termo,
        //                            Periodo_Historico_Termo,
        //                            Localidade_Termo,
        //                            Informacao_Local,
        //                            Status_Item,
        //                            Status_Emprestimo,
        //                            Id_Autor,
        //                            Id_Entidade
        //                        ) VALUES (
        //                            @ISBN,
        //                            @Condicoes_Encadernacao,
        //                            @Agencia_Catalogadora,
        //                            @Idioma_Catalogacao,
        //                            @Agencia_Transcricao,
        //                            @Agencia_Modificacao,
        //                            @Idioma_Texto,
        //                            @Idioma_Resumo,
        //                            @Idioma_Legenda,
        //                            @Numero_CDD,
        //                            @Numero_Item_CDD,
        //                            @Numero_Chamada_Local,
        //                            @Numero_Item_Local,
        //                            @Numero_Chamada_Secundaria,
        //                            @Nome_Livro,
        //                            @Subtitulo,
        //                            @Indicacao_Responsabilidade,
        //                            @Indicador_Artigo_Inicial,
        //                            @Numero_Edicao,
        //                            @Mencao_Responsabilidade_Edicao,
        //                            @Local_Publicacao,
        //                            @Editora,
        //                            @Ano_Publicacao,
        //                            @Paginas,
        //                            @Ilustracoes,
        //                            @Dimensoes,
        //                            @Material_Adicional,
        //                            @Titulo_Serie,
        //                            @Numero_Serie,
        //                            @Notas_Gerais,
        //                            @Nome_Pessoal_Assunto,
        //                            @Datas_Pessoal,
        //                            @Funcao_Pessoal,
        //                            @Topico_Pessoal,
        //                            @Titulo_Uniforme,
        //                            @Forma_Uniforme,
        //                            @Periodo_Historico_Uniforme,
        //                            @Localidade_Uniforme,
        //                            @Assunto_Termo,
        //                            @Forma_Termo,
        //                            @Periodo_Historico_Termo,
        //                            @Localidade_Termo,
        //                            @Informacao_Local,
        //                            @Status_Item,
        //                            @Status_Emprestimo,
        //                            @Id_Autor,
        //                            @Id_Entidade
        //                        );
        //                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        //                        using (SqlCommand comandLA = new SqlCommand(insertLivroQuery, connection, transaction))
        //                        {
        //                            comandLA.Parameters.AddWithValue("@ISBN", CreateL.livro.ISBN);
        //                            comandLA.Parameters.AddWithValue("@Condicoes_Encadernacao", CreateL.livro.Cond_Encardenacao);
        //                            comandLA.Parameters.AddWithValue("@Agencia_Catalogadora", CreateL.livro.Agen_Catalogadora);
        //                            comandLA.Parameters.AddWithValue("@Idioma_Catalogacao", CreateL.livro.Idi_Catalogacao);
        //                            comandLA.Parameters.AddWithValue("@Agencia_Transcricao", CreateL.livro.Agen_Transcricao);
        //                            comandLA.Parameters.AddWithValue("@Agencia_Modificacao", CreateL.livro.Agen_Modificacao);
        //                            comandLA.Parameters.AddWithValue("@Idioma_Texto", CreateL.livro.Idi_Texto);
        //                            comandLA.Parameters.AddWithValue("@Idioma_Resumo", CreateL.livro.Idi_Resumo);
        //                            comandLA.Parameters.AddWithValue("@Idioma_Legenda", CreateL.livro.Idi_Legenda);
        //                            comandLA.Parameters.AddWithValue("@Numero_CDD", CreateL.livro.Numero_CDD);
        //                            comandLA.Parameters.AddWithValue("@Numero_Item_CDD", CreateL.livro.Numero_Item_CDD);
        //                            comandLA.Parameters.AddWithValue("@Numero_Chamada_Local", CreateL.livro.Num_Cham_Local);
        //                            comandLA.Parameters.AddWithValue("@Numero_Item_Local", CreateL.livro.Num_Item_Local);
        //                            comandLA.Parameters.AddWithValue("@Numero_Chamada_Secundaria", CreateL.livro.Num_Cham_Secundaria);
        //                            comandLA.Parameters.AddWithValue("@Nome_Livro", CreateL.livro.Nome);
        //                            comandLA.Parameters.AddWithValue("@Subtitulo", CreateL.livro.Subtitulo);
        //                            comandLA.Parameters.AddWithValue("@Indicacao_Responsabilidade", CreateL.livro.Indi_Responsabilidade);
        //                            comandLA.Parameters.AddWithValue("@Indicador_Artigo_Inicial", CreateL.livro.Indi_Arti_Inicial);
        //                            comandLA.Parameters.AddWithValue("@Numero_Edicao", CreateL.livro.Num_Edicao);
        //                            comandLA.Parameters.AddWithValue("@Mencao_Responsabilidade_Edicao", CreateL.livro.Mencao_Responsa_Edicao);
        //                            comandLA.Parameters.AddWithValue("@Local_Publicacao", CreateL.livro.Local_Publicacao);
        //                            comandLA.Parameters.AddWithValue("@Editora", CreateL.livro.Editora);
        //                            comandLA.Parameters.AddWithValue("@Ano_Publicacao", CreateL.livro.Ano_Publicacao);
        //                            comandLA.Parameters.AddWithValue("@Paginas", CreateL.livro.Paginas);
        //                            comandLA.Parameters.AddWithValue("@Ilustracoes", CreateL.livro.Ilustracoes);
        //                            comandLA.Parameters.AddWithValue("@Dimensoes", CreateL.livro.Dimensoes);
        //                            comandLA.Parameters.AddWithValue("@Material_Adicional", CreateL.livro.Material_Adicional);
        //                            comandLA.Parameters.AddWithValue("@Titulo_Serie", CreateL.livro.Titulo_Serie);
        //                            comandLA.Parameters.AddWithValue("@Numero_Serie", CreateL.livro.Num_Serie);
        //                            comandLA.Parameters.AddWithValue("@Notas_Gerais", CreateL.livro.Notas_Gerais);
        //                            comandLA.Parameters.AddWithValue("@Nome_Pessoal_Assunto", CreateL.livro.Nome_Pess_Assunto);
        //                            comandLA.Parameters.AddWithValue("@Datas_Pessoal", CreateL.livro.Datas_Pessoais);
        //                            comandLA.Parameters.AddWithValue("@Funcao_Pessoal", CreateL.livro.Funcao_Pessoal);
        //                            comandLA.Parameters.AddWithValue("@Topico_Pessoal", CreateL.livro.Topico);
        //                            comandLA.Parameters.AddWithValue("@Titulo_Uniforme", CreateL.livro.Titulo_Uniforme);
        //                            comandLA.Parameters.AddWithValue("@Forma_Uniforme", CreateL.livro.Forma_Uniforme);
        //                            comandLA.Parameters.AddWithValue("@Periodo_Historico_Uniforme", CreateL.livro.Periodo_Historico);
        //                            comandLA.Parameters.AddWithValue("@Localidade_Uniforme", CreateL.livro.Local_Uniforme);
        //                            comandLA.Parameters.AddWithValue("@Assunto_Termo", CreateL.livro.Assunto_Termo);
        //                            comandLA.Parameters.AddWithValue("@Forma_Termo", CreateL.livro.Forma_Termo);
        //                            comandLA.Parameters.AddWithValue("@Periodo_Historico_Termo", CreateL.livro.Periodo_Histo_Termo);
        //                            comandLA.Parameters.AddWithValue("@Localidade_Termo", CreateL.livro.Local_Termo);
        //                            comandLA.Parameters.AddWithValue("@Informacao_Local", CreateL.livro.Info_Local);
        //                            comandLA.Parameters.AddWithValue("@Status_Item", CreateL.livro.Status_Item);
        //                            comandLA.Parameters.AddWithValue("@Status_Emprestimo", CreateL.livro.Status_Emprestimos);

        //                            LivroId = (int)comandLA.ExecuteScalar();


        //                            comandLA.ExecuteNonQuery();
        //                        }

        //                        // Se tudo deu certo, confirma a transação
        //                        transaction.Commit();
        //                        return Ok(new { message = "Livro cadastrado com sucesso!" });
        //                    }
        //                }

        //            }
        //            if (CreateL.autor.Nome_Autor.ToString() == string.Empty || CreateL.autor.Nome_Autor.ToString() == null || CreateL.autor.Nome_Autor.ToString() == "")
        //            {
        //                string LivroAutorInsert = "INSERT INTO Livro_Autor (Id_Autor,Id_Livro) VALUES (@Id_Autor,@Id_Livro)";
        //                using(SqlCommand comandLA = new SqlCommand(LivroAutorInsert, connection, transaction))
        //                {
        //                    comandLA.Parameters.AddWithValue("@Id_Autor", autorId);
        //                    comandLA.Parameters.AddWithValue("@Id_Livro", entidadeId);
        //                }

        //            }
        //            else
        //            {
        //                string LivroEntidadeInsert = "INSERT INTO Livro_Entidade (Id_Autor,Id_Livro) VALUES (@Id_Entidade,@Id_Livro)";
        //                using (SqlCommand comandLA = new SqlCommand(LivroEntidadeInsert, connection, transaction))
        //                {
        //                    comandLA.Parameters.AddWithValue("@Id_Entidade", entidadeId);
        //                    comandLA.Parameters.AddWithValue("@Id_Livro", entidadeId);
        //                }
        //            }
        //        }


        //        catch (Exception ex)
        //        {
        //            // Se algo deu errado, desfaz todas as alterações
        //            transaction.Rollback();
        //            return BadRequest(new { message = "Erro ao cadastrar livro", error = ex.Message });
        //        }
        //    }
        //} 

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
                    Status_Emprestimos
                ) VALUES (
                    @Nome_Livro,
                    @Editora,
                    @Ano_Publicacao,
                    @Paginas,
                    @Notas_Gerais,
                    @Assunto_Termo,
                    @Status_Emprestimos
                )";

                    SqlCommand comand = new SqlCommand(query, conection);
                    comand.Parameters.AddWithValue("@Nome_Livro", livro.Nome);
                    comand.Parameters.AddWithValue("@Editora", livro.Editora);
                    comand.Parameters.AddWithValue("@Ano_Publicacao", livro.Ano_Publicacao);
                    comand.Parameters.AddWithValue("@Paginas", livro.Paginas);
                    comand.Parameters.AddWithValue("@Notas_Gerais", livro.Notas_Gerais);
                    comand.Parameters.AddWithValue("@Assunto_Termo", livro.Assunto_Termo);
                    comand.Parameters.AddWithValue("@Status_Emprestimos", livro.Status_Emprestimos);

                    conection.Open();
                    int rowsAffected = comand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }

        [HttpDelete("{id}")]
        [HttpDelete]
        public ActionResult DeleteLivro(int id)
        {
            using (SqlConnection connection = new SqlConnection(StrConex))
            {
                connection.Open();
                SqlCommand comand1 = new SqlCommand("DELETE FROM Livro_Autor WHERE Id_Livro = @Id", connection);
                comand1.Parameters.AddWithValue("@Id", id);
                comand1.ExecuteNonQuery();

                // Segundo DELETE
                SqlCommand comand2 = new SqlCommand("DELETE FROM Livro WHERE Id_Livro = @Id", connection);
                comand2.Parameters.AddWithValue("@Id", id);
                int rowsAffected = comand2.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return Ok();
                }
            }
            return NotFound();
        }
    }
    } 


