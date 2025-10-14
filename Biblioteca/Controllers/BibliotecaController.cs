using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BibliotecaController : Controller
    {
        private readonly ILogger<BibliotecaController> _logger;

        private const string StrConex = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Biblioteca;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public BibliotecaController(ILogger<BibliotecaController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "GetLivroTotal")]
        public IEnumerable<Biblioteca> GetLivro()
        {
            List<Biblioteca> bibliotecas = new List<Biblioteca>();

            using (SqlConnection conection = new SqlConnection(StrConex))
            {
                string query = @"
            SELECT 
                l.*,
                a.Id_Autor,
                a.Nome_Autor,
                a.Numero,
                a.Datas,
                a.Funcao,
                a.Tipo_Autor
            FROM Livro l
            LEFT JOIN Livro_Autor la ON l.Id_Livro = la.Id_Livro
            LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
            LEFT JOIN Livro_Entidade le  ON l.Id_Livro = le.Id_Entidade
            LEFT JOIN Entidade_Corporativa e  ON le.Id_Entidade = e.Id_Entidade
            ORDER BY l.Id_Livro";

                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Biblioteca bibliotecaAtual = null;
                int idLivroAnterior = -1;

                while (reader.Read())
                {
                    int idLivro = Convert.ToInt32(reader["Id_Livro"]);

                    // Se é um novo livro, cria um novo objeto Biblioteca
                    if (idLivro != idLivroAnterior)
                    {
                        if (bibliotecaAtual != null)
                        {
                            bibliotecas.Add(bibliotecaAtual);
                        }

                        bibliotecaAtual = new Biblioteca()
                        {
                            Livro = new Livro()
                            {
                                Id = idLivro,
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
                            },
                            Autores = new List<Autor>(),
                            Entidades = new List<Entidade_Corporativa>()
                        };

                        idLivroAnterior = idLivro;
                    }

                    // Adiciona o autor se existir
                    if (reader["Id_Autor"] != DBNull.Value)
                    {
                        Autor autor = new Autor()
                        {
                            Id = Convert.ToInt32(reader["Id_Autor"]),
                            Nome_Autor = reader["Nome_Autor"]?.ToString() ?? string.Empty,
                            Numero = reader["Numero"]?.ToString() ?? string.Empty,
                            Datas = reader["Datas"]?.ToString() ?? string.Empty,
                            Funcao = reader["Funcao"]?.ToString() ?? string.Empty,
                            Tipo_Autor = reader["Tipo_Autor"]?.ToString() ?? string.Empty
                        };

                        bibliotecaAtual.Autores.Add(autor);
                    }

                    //if (reader["Id_Entidade"] != DBNull.Value)
                    //{
                    //    Entidade_Corporativa entidade = new Entidade_Corporativa()
                    //    {
                    //        Id = Convert.ToInt32(reader["Id_Autor"]),
                    //        Nome_Entidade = reader["Nome_Entidade"]?.ToString() ?? string.Empty,
                    //        Subordinacao = reader["Subordinacao"]?.ToString() ?? string.Empty,
                    //    };

                    //    bibliotecaAtual.Entidades.Add(entidade);
                    //}
                }

                // Adiciona o último item
                if (bibliotecaAtual != null)
                {
                    bibliotecas.Add(bibliotecaAtual);
                }

                reader.Close();
            }

            return bibliotecas;
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

    }
}



