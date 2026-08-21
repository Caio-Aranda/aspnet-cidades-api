using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Projeto.Entidades; 
using Projeto.Services;
using Projeto.Repository;  

namespace Projeto.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadesController : ControllerBase
    {
        private readonly CidadeServices _services;
        public CidadesController(CidadeServices services)
        {
            _services = services;
            
        }
      
        [HttpPost("importar")]
        public IActionResult ImportarCsv(IFormFile arquivo)
        {
            try
            {
                if (arquivo == null || arquivo.Length == 0)
                    return BadRequest("Arquivo não enviado ou vazio.");

                // Chama a mágica que está lá no Service
                var sucesso = _services.ImportarCsv(arquivo);

                if (sucesso)
                    return Ok(new { mensagem = "Cidades importadas com sucesso!" });
                else
                    return BadRequest("O arquivo estava vazio ou com erro.");
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        [HttpGet]
        public IActionResult ObterTodas()
        {
            try { return Ok(_services.ObterTodas()); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("total")]
        public IActionResult ObterTotal()
        {
            try { return Ok(new { Total = _services.TotalCidades() }); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            try
            {
                var cidade = _services.Obter(id);
                return cidade != null ? Ok(cidade) : NotFound("Cidade não encontrada.");
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("estados")]
        public IActionResult ObterEstados()
        {
            try { return Ok(_services.ObterEstados()); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("estado/{uf}")]
        public IActionResult ObterPorEstado(string uf)
        {
            try { return Ok(_services.ObterPorEstado(uf)); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int id, [FromBody] Cidade cidade)
        {
            try
            {
                if (id != cidade.CidadeId) return BadRequest("IDs não conferem.");
                if (_services.Obter(id) == null) return NotFound();
                _services.Alterar(cidade);
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            try
            {
                if (_services.Obter(id) == null) return NotFound();
                _services.Excluir(id);
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
    }
}