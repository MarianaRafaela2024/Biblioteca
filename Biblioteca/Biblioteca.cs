namespace Biblioteca
{
    public class Biblioteca
    {
        public Livro Livro { get; set; }
        public List<Autor> Autores { get; set; } = new List<Autor>();
        public List<Entidade_Corporativa> EntidadesB { get; set; } = new List<Entidade_Corporativa>();
    }
}
