using Microsoft.AspNetCore.SignalR;

namespace folhaPagamento.Models;

public class Folha
{
    public int FolhaId { get; set; }
    public double Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public Funcionario? Funcionario { get; set;}
    public int FuncionarioId { get; set;}
}
