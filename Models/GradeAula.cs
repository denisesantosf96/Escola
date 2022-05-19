namespace Escola.Models
{
    public class GradeAula
    {
        public int Id { get; set; }
        public int IdTurma { get; set; }
        public int IdProfessor { get; set; }
        public int IdMateria { get; set; }
        public int IdEscola { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string Serie { get; set; }
    }
}