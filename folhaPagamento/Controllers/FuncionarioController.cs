namespace folhaPagamento.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using folhaPagamento.Data;
using folhaPagamento.Models;

[ApiController]
[Route("api/funcionario")]
public class FuncionarioController : ControllerBase
{
    private readonly AppDataContext _ctx;
    public FuncionarioController(AppDataContext context)
    {
        _ctx = context;
    }

    // GET: api/funcionario/listar
    [HttpGet]
    [Route("listar")]
    public ActionResult Listar()
    {
        try
        {
            List<Funcionario> funcionarios = _ctx.Funcionarios.ToList();
            return funcionarios.Count == 0 ? NotFound() : Ok(funcionarios);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/funcionario/buscar/{nome}
    [HttpGet]
    [Route("buscar/{nome}")]
    public ActionResult Buscar([FromRoute] string nome)
    {
        try
        {
            Funcionario? funcionarioCadastrado = _ctx.Funcionarios.FirstOrDefault(x => x.Nome == nome);
            if (funcionarioCadastrado != null)
            {
                return Ok(funcionarioCadastrado);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/funcionario/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public ActionResult Cadastrar([FromBody] Funcionario funcionario)
    {
        try
        {
            _ctx.Funcionarios.Add(funcionario);
            _ctx.SaveChanges();
            return Created("", funcionario);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // PUT: api/funcionario/alterar/5
    [HttpPut]
    [Route("alterar/{id}")]
    public IActionResult Alterar([FromRoute] int id,
        [FromBody] Funcionario funcionario)
    {
        try
        {
            Funcionario? funcionarioCadastrado =
                _ctx.Funcionarios.FirstOrDefault(x => x.FuncionarioId == id);
            if (funcionarioCadastrado != null)
            {
                funcionarioCadastrado.Nome = funcionario.Nome;
                funcionarioCadastrado.Cpf = funcionario.Cpf;
                _ctx.SaveChanges();
                return Ok(funcionario);
            }
            return NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // DELETE: api/funcionario/deletar/5
    [HttpDelete]
    [Route("deletar/{id}")]
    public IActionResult Deletar([FromRoute] int id)
    {
        try
        {
            Funcionario? funcionarioCadastrado = _ctx.Funcionarios.Find(id);
            if (funcionarioCadastrado != null)
            {
                _ctx.Funcionarios.Remove(funcionarioCadastrado);
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
