namespace Escola.Models
{
    public class Boletim
    {
        public int Id { get; set; }
        public int IdAluno { get; set; }
        public int IdGradeAula { get; set; }
        public string DescricaoAvaliacao { get; set; }    
        public decimal Nota { get; set; } 
        public string Nome { get; set; }
        public int IdEscola { get; set; }
        public int IdTurma { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string Serie { get; set; }

    }
}