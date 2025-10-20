namespace Biblioteca
{
    public class Biblioteca
    {
        public Livro Livro { get; set; }
        public List<Autor> Autores { get; set; } = new List<Autor>();
        public List<Entidade_Corporativa> Entidades { get; set; } = new List<Entidade_Corporativa>();

        public List<Exemplares_Automaticos> Exemplares { get; set; } = new List<Exemplares_Automaticos>();
    }
}
