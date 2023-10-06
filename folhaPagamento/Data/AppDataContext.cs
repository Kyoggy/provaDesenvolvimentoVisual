using Microsoft.EntityFrameworkCore;
using folhaPagamento.Models;

namespace folhaPagamento.Data;

public class AppDataContext : DbContext
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options) 
    {

    }

    //Classes que vão virar tabelas no banco de dados
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Folha> Folhas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Como popular uma base de dados utilizando EF no método
        //OnModelCreating, quero dados reais de Funcionario, com os seguintes
        //atributos


        base.OnModelCreating(modelBuilder);
    }
}
