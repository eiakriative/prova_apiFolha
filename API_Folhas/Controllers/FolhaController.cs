using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using API_Folhas.Models;

namespace API_Folhas.Controllers
{
    [ApiController]
    [Route("api/folha")]
    public class FolhaController : ControllerBase
    {
        private readonly DataContext _context;

        public FolhaController(DataContext context)
        {
            _context = context;
        }

        // GET: /api/folha/listar
        [HttpGet]
        [Route("listar")]
        public IActionResult Listar() => Ok(_context.Folhas.Include(a => a.Funcionario).ToList());

        // POST: /api/folha/cadastrar
        [HttpPost]
        [Route("cadastrar")]
        public IActionResult Cadastrar([FromBody] FolhaPagamento folhaPag)
        {
            folhaPag.Bruto = folhaPag.QtdeHoras * folhaPag.ValHora;

            if(folhaPag.Bruto < 1903.98){
                folhaPag.IR = 0;
            }else if(folhaPag.Bruto > 1903.99 && folhaPag.Bruto < 2826.65){
                folhaPag.IR = 142.80;
            }else if(folhaPag.Bruto > 2826.66 && folhaPag.Bruto < 3751.05){
                folhaPag.IR = 354.80;
            }else if(folhaPag.Bruto > 3751.06 && folhaPag.Bruto < 4664.68){
                folhaPag.IR = 636.13;
            }else{
                folhaPag.IR = 869.36;
            }

            if(folhaPag.Bruto < 1693.72){
                folhaPag.Inss = folhaPag.Bruto % 8;
            }else if(folhaPag.Bruto > 1693.73 && folhaPag.Bruto < 2822.90){
                folhaPag.Inss = folhaPag.Bruto % 9;
            }else if(folhaPag.Bruto > 2822.91  && folhaPag.Bruto < 5645.80){
                folhaPag.Inss = folhaPag.Bruto % 11;
            }else{
                folhaPag.Inss = folhaPag.Bruto - 621.03;
            }

            folhaPag.Fgts = folhaPag.Bruto % 8;

            folhaPag.Liquido = folhaPag.Bruto - folhaPag.Inss - folhaPag.IR;

            if(folhaPag != null && _context.Funcionarios.Any
            (
                a => a.FuncionarioId == folhaPag.FuncionarioId)
            )
            {
                _context.Folhas.Add(folhaPag);
                _context.SaveChanges();
                return Created("", folhaPag);
            }
            else
                return NotFound();
        }

        // GET: /api/folhas/buscar/{cpf}/{Nascimento}
         // GET: /api/folha/buscar/{cpf}
        [HttpGet]
        [Route("buscar/{cpf}/{mes}/{ano}")]
        public IActionResult Buscar([FromRoute] string cpf, int mes, int ano)
        {
            Funcionario funcionario = _context.Funcionarios.
                FirstOrDefault(f => f.Cpf.Equals(cpf));
            FolhaPagamento folha = _context.Folhas.
                FirstOrDefault(
                    f => f.Funcionario.Equals(funcionario) && 
                    f.mes.Equals(mes) && f.ano.Equals(ano));

            return folha != null ? Ok(folha) : NotFound();
        }

        // GET: /api/folha/filtrar/{cpf}/{mes}/{ano}
        [HttpGet]
        [Route("filtrar/{mes}/{ano}")]
        public IActionResult Filtrar([FromRoute] int mes, int ano)
        {
            return Ok(_context.Folhas.Include(f => f.Funcionario)
            .Where(f=> f.mes.Equals(mes) && f.ano.Equals(ano)).ToList());
        }


    }
}