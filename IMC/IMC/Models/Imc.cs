namespace IMC.Models;

public class Imc {

    public string ImcId { get; set; } = Guid.NewGuid().ToString();
    public Aluno? Aluno { get; set; }
    public string? AlunoId { get; set; }
    public double Altura { get; set; }
    public double Peso { get; set; }
    public double Valor { get; set; }
    public string Classificacao { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
}