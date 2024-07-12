using Microsoft.EntityFrameworkCore;

namespace IMC.Models;

public class AppDataContext : DbContext
{
    public DbSet<Imc> Imcs { get; set; }
    public DbSet<Aluno> Alunos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=gabriela.db");
    }

}