using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using folhaPagamento.Data;
using folhaPagamento.DTOs;
using folhaPagamento.Models;

namespace folhaPagamento.Controllers;

[ApiController]
[Route("api/folha")]
public class FolhaController : ControllerBase
{
    private readonly AppDataContext _ctx;
    public FolhaController(AppDataContext ctx)
    {
        _ctx = ctx;
    }

    // GET: api/folha/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Folha> folhas = _ctx.Folhas
                .Include(f => f.Funcionario) // Inclui os dados do funcionário relacionado
                .ToList();

            // Calcula o salário bruto, imposto de renda, INSS, FGTS e salário líquido para cada folha
            var folhasComInformacoesExtras = folhas.Select(folha => new
            {
                folha.FolhaId,
                folha.Valor,
                folha.Quantidade,
                folha.Mes,
                folha.Ano,
                SalarioBruto = folha.Valor * folha.Quantidade,                
                ImpostoDeRenda = CalcularImpostoDeRenda(folha), // Substitua por sua lógica de cálculo
                INSS = CalcularINSS(folha), // Substitua por sua lógica de cálculo
                FGTS = CalcularFGTS(folha), // Substitua por sua lógica de cálculo
                SalarioLiquido = CalcularSalarioLiquido(folha), // Substitua por sua lógica de cálculo
                Funcionario = new
                {
                    folha.Funcionario.FuncionarioId,
                    folha.Funcionario.Nome,
                    folha.Funcionario.Cpf
                    // Adicione outros campos do funcionário que desejar
                },
                folha.FuncionarioId
            }).ToList();

            return folhasComInformacoesExtras.Count == 0 ? NotFound() : Ok(folhasComInformacoesExtras);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Função para calcular o Imposto de Renda
    private double CalcularImpostoDeRenda(Folha folha)
    {   
        // Suponhamos uma alíquota fixa de 15% para o Imposto de Renda
        double salarioBruto = folha.Valor * folha.Quantidade;
        if( salarioBruto <= 1903.98)
        {
            double aliquota = 0;
            return aliquota;
        }
        else if( salarioBruto <= 2826.65)
        {
            double aliquota = 0.075;
            return salarioBruto * aliquota;
        }
        else if( salarioBruto <= 3751.05)
        {
            double aliquota = 0.15;
            return salarioBruto * aliquota;
        }
        else if( salarioBruto <= 4664.68)
        {
            double aliquota = 0.225;
            return salarioBruto * aliquota;
        }
        else
        {
            double aliquota = 0.275;
            return salarioBruto * aliquota;
        }

        }
    

    // Função para calcular o INSS
    private double CalcularINSS(Folha folha)
    {
        // Suponhamos uma alíquota fixa de 10% para o INSS
        double salarioBruto = folha.Valor * folha.Quantidade;
        if (salarioBruto <= 1693.72)
        {
            double inss = 0.08;
            return salarioBruto * inss;
        }
        else if (salarioBruto <= 2822.90)
        {
            double inss = 0.09;
            return salarioBruto * inss;
        }
        else if (salarioBruto <= 5645.80)
        {
            double inss = 0.11;
            return salarioBruto * inss;
        }
        else
        {            
            return 621.03;
        }
    }

    // Função para calcular o FGTS
    private double CalcularFGTS(Folha folha)
    {
        // Suponhamos uma alíquota fixa de 8% para o FGTS
        double salarioBruto = folha.Valor * folha.Quantidade;
        double aliquotaFGTS = 0.08; // 8%
        return salarioBruto * aliquotaFGTS;
    }

    // Função para calcular o Salário Líquido
    private double CalcularSalarioLiquido(Folha folha)
    {
        double salarioBruto = folha.Valor * folha.Quantidade;
        double impostoDeRenda = CalcularImpostoDeRenda(folha);
        double inss = CalcularINSS(folha);
        double fgts = CalcularFGTS(folha);

        // Suponhamos que o salário líquido seja o salário bruto menos os descontos de IR, INSS e FGTS
        double salarioLiquido = salarioBruto - impostoDeRenda - inss - fgts;
        return salarioLiquido;
    }

    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] FolhaDTO folhaDTO)
    {
        try
        {
            Funcionario? funcionario =
                _ctx.Funcionarios.Find(folhaDTO.FuncionarioId);
            if (funcionario == null)
            {
                return NotFound();
            }
            Folha folha = new Folha
            {
                Valor = folhaDTO.Valor,
                Quantidade = folhaDTO.Quantidade,
                Mes = folhaDTO.Mes,
                Ano = folhaDTO.Ano,
                Funcionario = funcionario,
                FuncionarioId = folhaDTO.FuncionarioId
            };
            _ctx.Folhas.Add(folha);
            _ctx.SaveChanges();
            return Created("", folha);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("buscar/{cpf}/{mes}/{ano}")]
    public IActionResult Buscar([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano)
    {
        try
        {
            Funcionario funcionario = _ctx.Funcionarios.FirstOrDefault(x => x.Cpf == cpf);
            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado");
            }

            // Agora, apenas busca a folha do funcionário especificado
            Folha? folhaCadastrada = _ctx.Folhas.FirstOrDefault(x => x.Mes == mes && x.Ano == ano && x.FuncionarioId == funcionario.FuncionarioId);
            if (folhaCadastrada != null)
            {
                return Ok(new
                {            
                    folhaCadastrada.FolhaId,
                    folhaCadastrada.Valor,
                    folhaCadastrada.Quantidade,
                    folhaCadastrada.Mes,
                    folhaCadastrada.Ano,
                    SalarioBruto = folhaCadastrada.Valor * folhaCadastrada.Quantidade,
                    ImpostoDeRenda = CalcularImpostoDeRenda(folhaCadastrada), // Substitua por sua lógica de cálculo
                    INSS = CalcularINSS(folhaCadastrada), // Substitua por sua lógica de cálculo
                    FGTS = CalcularFGTS(folhaCadastrada), // Substitua por sua lógica de cálculo
                    SalarioLiquido = CalcularSalarioLiquido(folhaCadastrada), // Substitua por sua lógica de cálculo
                    Funcionario = new
                    {
                        funcionario.FuncionarioId,
                        funcionario.Nome,
                        funcionario.Cpf
                        // Adicione outros campos do funcionário que desejar
                    },
                    folhaCadastrada.FuncionarioId
                });
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Folha? folhaCadastrada = _ctx.Folhas.Find(id);
            if (folhaCadastrada != null)
            {
                _ctx.Folhas.Remove(folhaCadastrada);
                _ctx.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id,
        [FromBody] Folha folha)
    {
        try
        {
            Folha? folhaCadastrada =
                _ctx.Folhas.FirstOrDefault(x => x.FolhaId == id);
            if (folhaCadastrada != null)
            {
                folhaCadastrada.Valor = folha.Valor;
                folhaCadastrada.Quantidade = folha.Quantidade;
                folhaCadastrada.Mes = folha.Mes;
                folhaCadastrada.Ano = folha.Ano;
                _ctx.Folhas.Update(folhaCadastrada);
                _ctx.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
