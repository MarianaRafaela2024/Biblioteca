using System.ComponentModel.DataAnnotations;

namespace Biblioteca
{
    public class Emprestimo
    {
        public int Id_Emprestimo { get; set; }
        public string Rm_Aluno { get; set; }
        public int Id_Livro { get; set; }
        //public int? Id_Bibliotecario { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data_Emprestimo { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data_Devolucao_Prevista { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Data_Devolucao_Real { get; set; }
    }
}
