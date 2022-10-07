using System;

namespace API_Folhas.Models
{
    public class FolhaPagamento
    {
        public FolhaPagamento () => CriadoEm = DateTime.Now;
        public int FolhaPagamentoId { get; set; }
        public int ValHora { get; set; }
        public int QtdeHoras { get; set; }
        public int Bruto { get; set; }
        public Double IR { get; set; }
        public Double Inss { get; set; }
        public Double Fgts { get; set; }
        public Double Liquido { get; set; }

        public int mes {get; set;}
        public int ano {get; set;}
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}