using Biblioteca.Controllers;

namespace Biblioteca
{
    public class BibliotecaRequest
    {
            public Livro livro { get; set; }
            public Autor autor { get; set; }
            public Entidade_Corporativa entidade { get; set; }
            
            public Livro_Autor Livro_Autor { get; set; }

            public Exemplares_Automaticos Exemplares {  get; set; }
    }
}
