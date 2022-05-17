namespace Escola.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public int IdEscola { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string Serie { get; set; }
        public int Ano { get; set; }
    }
}