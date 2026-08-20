
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto.Controllers.DTOS;
using Projeto.Entidades;
using Projeto.Services;

namespace Projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly AlunoServices _services;

        public AlunosController(AlunoServices services)
        {
            _services = services;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id inválido");

                var alunoExistente = _services.Obter(id);
                if (alunoExistente != null)
                {
                    var response = new AlunoResponse
                    {
                        Id = alunoExistente.Id,
                        Nome = alunoExistente.Nome,
                        Idade = alunoExistente.Idade,
                        CidadeId = alunoExistente.Cidade.CidadeId
                    };
                    return Ok(response);
                }
                return NotFound("Não encontrado");
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            try
            {
                var alunos = _services.ObterTodos();
                var responseList = alunos.Select(item => new AlunoResponse
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Idade = item.Idade,
                    CidadeId = item.Cidade.CidadeId
                });
                return Ok(responseList);
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("consultar")]
        public IActionResult Consultar(string nome)
        {
            try
            {
                var alunos = _services.Consultar(nome);
                var responseList = alunos.Select(item => new AlunoResponse
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Idade = item.Idade,
                    CidadeId = item.Cidade.CidadeId
                });
                return Ok(responseList);
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("quantidade")]
        public IActionResult ObterQuantidade()
        {
            try { return Ok(new { Total = _services.TotalAlunos() }); }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpPost]
        public IActionResult Gravar(AlunoCriarRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Nome)) return BadRequest("Nome inválido.");

                var aluno = new Aluno
                {
                    Nome = request.Nome,
                    Idade = request.Idade,
                    Cidade = new Cidade { CidadeId = request.CidadeId }
                };

                if (_services.Criar(aluno)) return CreatedAtAction(nameof(Obter), new { id = aluno.Id }, aluno);

                return BadRequest("Falha ao gravar aluno.");
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int id, AlunoAlterarRequest request)
        {
            try
            {
                if (id <= 0) return BadRequest("Id inválido");
                if (!_services.AlunoExistente(id)) return NotFound();

                var aluno = new Aluno
                {
                    Id = id,
                    Nome = request.Nome,
                    Idade = request.Idade,
                    Cidade = new Cidade { CidadeId = request.CidadeId }
                };

                _services.Alterar(aluno);
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpPatch("{id}")]
        public IActionResult AlterarParcial(int id, AlunoAlterarParcialRequest request)
        {
            try
            {
                if (id <= 0) return BadRequest("Id inválido");

                var aluno = _services.Obter(id);
                if (aluno == null) return NotFound();

                if (!string.IsNullOrEmpty(request.Nome)) aluno.Nome = request.Nome;
                if (request.CidadeId != null) aluno.Cidade = new Cidade { CidadeId = (int)request.CidadeId };
                if (request.Idade != null && request.Idade > 0) aluno.Idade = (int)request.Idade;

                _services.Alterar(aluno);
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            try
            {
                if (id <= 0) return BadRequest("Id inválido");
                if (!_services.AlunoExistente(id)) return NotFound();

                _services.Excluir(id);
                return NoContent();
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        // DESAFIO 2: UPLOAD E LEITURA DE IMAGEM
        [HttpPost("{id}/foto")]
        public IActionResult UploadFoto(int id, IFormFile foto)
        {
            try
            {
                if (foto == null || foto.Length == 0) return BadRequest("Foto inválida.");
                if (!_services.AlunoExistente(id)) return NotFound("Aluno não encontrado.");

                if (_services.SalvarFoto(id, foto)) return Ok("Foto armazenada com sucesso.");
                return BadRequest("Erro ao salvar foto.");
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }

        [HttpGet("{id}/foto")]
        public IActionResult ObterFoto(int id)
        {
            try
            {
                if (!_services.AlunoExistente(id)) return NotFound("Aluno não encontrado.");

                var fotoBase64 = _services.ObterFoto(id);
                if (string.IsNullOrEmpty(fotoBase64)) return NotFound("O aluno não possui foto cadastrada.");

                return Ok(new { Base64 = fotoBase64 });
            }
            catch (Exception ex) { return Problem(detail: ex.Message, statusCode: 500); }
        }
    }
}