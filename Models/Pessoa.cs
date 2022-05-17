using System;

namespace Escola.Models
{
    public class Pessoa
    {
       public int Acao { get; set; }
       public string Opcao { get; set; }
       public int Id { get; set; }
       public string Nome { get; set; }
       public string CPF { get; set; }
       public string RG { get; set; }
       public string Telefone { get; set; }
       public string Endereco { get; set; }
       public decimal Numero { get; set; }
       public string Complemento { get; set; }
       public string Bairro { get; set; }
       public string Cidade { get; set; }
       public string Estado { get; set; }
       public string Pais { get; set; }
       public string CEP { get; set; }
       public DateTime DataNascimento { get; set; }
       public string NomeMae { get; set; }
       public string NomePai { get; set; }
       public int IdTurma { get; set; }
    }
}