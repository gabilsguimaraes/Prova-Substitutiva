using IMC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

app.MapGet("/", () => "Prova Substitutiva");

//Cadastrar  Aluno
//POST: http://localhost:5273/pages/aluno/cadastrar
app.MapPost("/pages/aluno/cadastrar", ([FromServices] AppDataContext ctx, [FromBody] Aluno aluno) =>
{

    Aluno? alunoBuscado = ctx.Alunos.
        FirstOrDefault(x => x.Nome == aluno.Nome && x.Sobrenome == aluno.Sobrenome);
    if (alunoBuscado is not null)
    {
        return Results.
            BadRequest("Já existe um aluno com o mesmo nome e sobrenome!");
    }

    ctx.Alunos.Add(aluno);
    ctx.SaveChanges();
    return Results.Created("", aluno);
});

//Cadastrar IMC
//POST: http://localhost:5273/pages/imc/cadastrar
app.MapPost("/pages/imc/cadastrar", ([FromServices] AppDataContext ctx, [FromBody] Imc imc) =>
{
    //Validar se o aluno existe
     Aluno? aluno =
         ctx.Alunos.Find(imc.AlunoId);

     if (aluno is null)
         return Results.NotFound("Aluno não encontrado");

    imc.Aluno = aluno;

    //Calcular o IMC
    imc.Valor = imc.Peso / (imc.Altura * imc.Altura);

    //Classificação
    if (imc.Valor < 18.5)
        imc.Classificacao = "Magreza";
    if (imc.Valor <= 24.9)
        imc.Classificacao = "Normal";
    if (imc.Valor <= 39.9)
        imc.Classificacao = "Obesidade";    
    else 
        imc.Classificacao = "Obesidade Grave";
    
    imc.Aluno = ctx.Alunos.Find(imc.AlunoId);
    
    ctx.Imcs.Add(imc);
    ctx.SaveChanges();
    return Results.Created("", imc);
});

//Listar IMC
//GET: http://localhost:5273/pages/imc/listar
app.MapGet("/pages/imc/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Imcs.Any())
    {
        return Results.Ok(ctx.Imcs.ToList());
    }
    return Results.NotFound("Nenhum IMC encontrado");
});

//Listar IMC por aluno
//GET: http://localhost:5273/pages/imc/listarporaluno
app.MapGet("/pages/imc/listarporaluno", ([FromServices] AppDataContext ctx) =>
{
    return Results.Ok(ctx.Imcs.Include(x => x.Aluno).ToList()); 
});

/*
//Alterar IMC
//PUT: http://localhost:5273/pages/imc/alterar/{id}
app.MapPut("/imc/alterar/{id}", ([FromServices] AppDataContext ctx, [FromRoute] string id) =>
{
    Imc? imc = ctx.Imc.Find(id);
    if (imc is null)
    {
        return Results.NotFound("IMC não encontrado");
    }

    imc.Altura = imcAtualizado.Altura;  
    imc.Peso = imcAtualizado.Peso;

    ctx.Imcs.Update(imc);
    ctx.SaveChanges();
    return Results.Ok(ctx.Imcs.ToList());
});
*/

app.UseCors("Acesso Total");
app.Run();
