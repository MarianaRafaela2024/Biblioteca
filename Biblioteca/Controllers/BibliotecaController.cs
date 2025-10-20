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
                a.Tipo_Autor,
                e.Id_Entidade,
                e.Nome_Entidade,
                e.Subordinacao,
                ex.*
            FROM Livro l
            LEFT JOIN Livro_Autor la ON l.Id_Livro = la.Id_Livro
            LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
            LEFT JOIN Livro_Entidade le  ON l.Id_Livro = le.Id_Livro
            LEFT JOIN Entidade_Corporativa e  ON le.Id_Entidade = e.Id_Entidade
            LEFT JOIN Exemplares_Automaticos ex  ON l.Id_Livro = ex.Id_Livro
            ORDER BY l.Id_Livro";

                SqlCommand command = new SqlCommand(query, conection);
                conection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Biblioteca bibliotecaAtual = null;
                int idLivroAnterior = -1;
                int idEntidadeAnterior = -1;
                int idExemplaresAnterior = -1;

                while (reader.Read())
                {
                    int idLivro = Convert.ToInt32(reader["Id_Livro"]);
                    int idExemplares = reader["Id_Exemplar_Auto"] != DBNull.Value ? Convert.ToInt32(reader["Id_Exemplar_Auto"]) : 0;
                    int idEntidade = reader["Id_Entidade"] != DBNull.Value ? Convert.ToInt32(reader["Id_Entidade"]): 0;

                    // Se é um novo livro, cria um novo objeto Biblioteca
                    if (idLivro != idLivroAnterior|| idEntidade != idEntidadeAnterior)
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
                                Status_Emprestimo = reader["Status_Emprestimo"]?.ToString() ?? string.Empty
                            },
                            Autores = new List<Autor>(),
                            Entidades = new List<Entidade_Corporativa>(),
                            Exemplares = new List<Exemplares_Automaticos>()
                        };

                        idLivroAnterior = idLivro;
                        idEntidadeAnterior = idEntidade;
                        idExemplaresAnterior = idExemplares;
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

                    if (reader["Id_Entidade"] != DBNull.Value)
                    {
                        Entidade_Corporativa entidade = new Entidade_Corporativa()
                        {
                            Id = Convert.ToInt32(reader["Id_Entidade"]),
                            Nome_Entidade = reader["Nome_Entidade"]?.ToString() ?? string.Empty,
                            Subordinacao = reader["Subordinacao"]?.ToString() ?? string.Empty,
                        };

                        bibliotecaAtual.Entidades.Add(entidade);
                    }

                    if (reader["Id_Exemplar_Auto"] != DBNull.Value)
                    {
                        Exemplares_Automaticos exemplar = new Exemplares_Automaticos()
                        {
                            Id = Convert.ToInt32(reader["Id_Exemplar_Auto"]),
                            Numero_Exemplares = Convert.ToInt32(reader["Numero_Exemplares"]),
                            Numero_Volume = Convert.ToInt32(reader["Numero_Volume"]),
                            Numero_Exemplares_Total = Convert.ToInt32(reader["Numero_Exemplares_Total"]),
                            Data_Aquisicao = reader["Data_Aquisicao"] != DBNull.Value ? DateOnly.FromDateTime(Convert.ToDateTime(reader["Data_Aquisicao"]))
                        : (DateOnly?)null,
                            Biblioteca_Depositaria = reader["Biblioteca_Depositaria"].ToString(),
                            Tipo_Aquisicao = reader["Tipo_Aquisicao"].ToString()


                        };

                        bibliotecaAtual.Exemplares.Add(exemplar);
                    }
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



        


        [HttpGet("search-complete")]
        public IEnumerable<Biblioteca> SearchComplete(string? termo)
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
                a.Tipo_Autor,
                e.Id_Entidade,
                e.Nome_Entidade,
                e.Subordinacao,
                ex.*
            FROM Livro l
            LEFT JOIN Livro_Autor la ON l.Id_Livro = la.Id_Livro
            LEFT JOIN Autor a ON la.Id_Autor = a.Id_Autor
            LEFT JOIN Livro_Entidade le ON l.Id_Livro = le.Id_Livro
            LEFT JOIN Entidade_Corporativa e ON le.Id_Entidade = e.Id_Entidade
            LEFT JOIN Exemplares_Automaticos ex ON l.Id_Livro = ex.Id_Livro
            WHERE (@termo IS NULL OR
                   l.Nome_Livro LIKE '%' + @termo + '%' OR
                   l.Subtitulo LIKE '%' + @termo + '%' OR
                   l.Indicacao_Responsabilidade LIKE '%' + @termo + '%' OR
                   l.Assunto_Termo LIKE '%' + @termo + '%' OR
                   l.ISBN LIKE '%' + @termo + '%' OR
                   CAST(l.Ano_Publicacao AS NVARCHAR) LIKE '%' + @termo + '%' OR
                   a.Nome_Autor LIKE '%' + @termo + '%' OR
                   e.Nome_Entidade LIKE '%' + @termo + '%')
            ORDER BY l.Id_Livro";

                SqlCommand command = new SqlCommand(query, conection);
                command.Parameters.AddWithValue("@termo", (object)termo ?? DBNull.Value);

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
                                Status_Emprestimo = reader["Status_Emprestimo"]?.ToString() ?? string.Empty
                            },
                            Autores = new List<Autor>(),
                            Entidades = new List<Entidade_Corporativa>(),
                            Exemplares = new List<Exemplares_Automaticos>()
                        };

                        idLivroAnterior = idLivro;
                    }

                    // Adiciona o autor se existir e ainda não foi adicionado
                    if (reader["Id_Autor"] != DBNull.Value)
                    {
                        int idAutor = Convert.ToInt32(reader["Id_Autor"]);

                        if (!bibliotecaAtual.Autores.Any(a => a.Id == idAutor))
                        {
                            Autor autor = new Autor()
                            {
                                Id = idAutor,
                                Nome_Autor = reader["Nome_Autor"]?.ToString() ?? string.Empty,
                                Numero = reader["Numero"]?.ToString() ?? string.Empty,
                                Datas = reader["Datas"]?.ToString() ?? string.Empty,
                                Funcao = reader["Funcao"]?.ToString() ?? string.Empty,
                                Tipo_Autor = reader["Tipo_Autor"]?.ToString() ?? string.Empty
                            };

                            bibliotecaAtual.Autores.Add(autor);
                        }
                    }

                    // Adiciona a entidade se existir e ainda não foi adicionada
                    if (reader["Id_Entidade"] != DBNull.Value)
                    {
                        int idEntidade = Convert.ToInt32(reader["Id_Entidade"]);

                        if (!bibliotecaAtual.Entidades.Any(e => e.Id == idEntidade))
                        {
                            Entidade_Corporativa entidade = new Entidade_Corporativa()
                            {
                                Id = idEntidade,
                                Nome_Entidade = reader["Nome_Entidade"]?.ToString() ?? string.Empty,
                                Subordinacao = reader["Subordinacao"]?.ToString() ?? string.Empty,
                            };

                            bibliotecaAtual.Entidades.Add(entidade);
                        }
                    }

                    // Adiciona o exemplar se existir e ainda não foi adicionado
                    if (reader["Id_Exemplar_Auto"] != DBNull.Value)
                    {
                        int idExemplar = Convert.ToInt32(reader["Id_Exemplar_Auto"]);

                        if (!bibliotecaAtual.Exemplares.Any(ex => ex.Id == idExemplar))
                        {
                            Exemplares_Automaticos exemplar = new Exemplares_Automaticos()
                            {
                                Id = idExemplar,
                                Numero_Exemplares = Convert.ToInt32(reader["Numero_Exemplares"]),
                                Numero_Volume = Convert.ToInt32(reader["Numero_Volume"]),
                                Numero_Exemplares_Total = Convert.ToInt32(reader["Numero_Exemplares_Total"]),
                                Data_Aquisicao = reader["Data_Aquisicao"] != DBNull.Value
                                    ? DateOnly.FromDateTime(Convert.ToDateTime(reader["Data_Aquisicao"]))
                                    : (DateOnly?)null,
                                Biblioteca_Depositaria = reader["Biblioteca_Depositaria"]?.ToString() ?? string.Empty,
                                Tipo_Aquisicao = reader["Tipo_Aquisicao"]?.ToString() ?? string.Empty
                            };

                            bibliotecaAtual.Exemplares.Add(exemplar);
                        }
                    }
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
    }
}



