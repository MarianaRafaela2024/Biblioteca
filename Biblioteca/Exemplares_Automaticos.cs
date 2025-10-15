namespace Biblioteca
{
    public class Exemplares_Automaticos
    {
        public int Id { get; set; }

        public int Id_Livro { get; set; }

        public int Numero_Exemplares { get; set; }
        
        public int Numero_Volume { get; set; }

        public int Numero_Exemplares_Total { get; set; }

        public DateOnly Data_Aquisicao { get; set; }

        public string Biblioteca_Depositaria { get; set; }
        
        public string Tipo_Aquisicao { get; set; }
    }
}
