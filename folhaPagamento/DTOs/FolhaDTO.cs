using System.ComponentModel.DataAnnotations;

namespace folhaPagamento.DTOs;
public class FolhaDTO
{

    public double Valor { get; set; }
    public int Quantidade { get; set; }
    public int Mes { get; set; }
    public int Ano { get; set; }
    public int FuncionarioId { get; set; }
}
