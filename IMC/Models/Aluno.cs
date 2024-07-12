namespace IMC.Models;

public class Aluno {

    public string AlunoId { get; set; } = Guid.NewGuid().ToString();
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }

}