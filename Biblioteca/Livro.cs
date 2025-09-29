using System.Globalization;

namespace Biblioteca
{
    public class Livro
    {
        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Cond_Encardenação { get; set; }

        public string Agen_Catalogadora { get; set; }

        public string Idi_Catalogocao { get; set; }

        public string Agen_Transcricao { get; set; }

        public string Agen_Modigicacao { get; set; }

        public string Idi_Texto { get; set; }

        public string Idi_Resumo { get; set; }
        
        public string Idi_Legenda { get; set; }

        public string Numero_CDD { get; set; }

        public string Numero_Item_CDD { get; set; }

        public string Num_Cham_Local { get; set; }

        public string Num_Item_Local { get; set; }

        public string Num_Cham_Secundaria { get; set; }

        public string Nome { get; set; }

        public string Subtitulo { get; set; }

        public string Indi_Responsabilidade { get; set; }

        public string Indi_Arti_Inicial { get; set; }

        public string Num_Edicao { get; set; }

        public string Mencao_Responsa_Edicao { get; set; }

        public string Local_Publicacao { get; set; }

        public string Editora { get; set; }

        public int Ano_Publicacao { get; set; }

        public string Paginas { get; set; }

        public string Ilustracoes { get; set; }

        public string Dimensoes { get; set; }

        public string Material_Adicional { get; set; }

        public string Titulo_Serie { get; set; }

        public string Num_Serie { get; set; }

        public string Notas_Gerais { get; set; }

        public string Nome_Pess_Assunto { get; set; }

        public string Datas_Pessoais { get; set; }

        public string Funcao_Pessoal { get; set; }

        public string Topico { get; set; }

        public string Titulo_Uniforme { get; set; }

        public string Forma_Uniforme { get; set; }

        public string Periodo_Historico { get; set; }

        public string Local_Uniforme { get; set; }

        public string Assunto_Termo { get; set; }

        public string Forma_Termo { get; set; }

        public string Periodo_Histo_Termo { get; set; }

        public string Local_Termo { get; set; }

        public string Info_Local { get; set; }

        public string Status_Item { get; set; }

        public string Status_Emprestimos { get; set; }
    }
}
