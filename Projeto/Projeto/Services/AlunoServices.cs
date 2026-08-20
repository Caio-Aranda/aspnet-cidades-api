
using Projeto.Entidades;
using Projeto.Repository;

namespace Projeto.Services
{
    public class AlunoServices
    {
        private readonly AlunoRepository _repository;

        public AlunoServices(AlunoRepository repository)
        {
            _repository = repository;
        }

        public bool Criar(Aluno aluno) => _repository.Criar(aluno);
        public bool Alterar(Aluno aluno) => _repository.Alterar(aluno);
        public Aluno? Obter(int id) => _repository.Obter(id);
        public IEnumerable<Aluno> ObterTodos() => _repository.ObterTodos();
        public IEnumerable<Aluno> Consultar(string nome) => _repository.Consultar(nome);
        public int TotalAlunos() => _repository.TotalAlunos();
        public void Excluir(int id) => _repository.Excluir(id);
        public bool AlunoExistente(string nome) => _repository.AlunoExistente(nome);
        public bool AlunoExistente(int id) => _repository.AlunoExistente(id);

        public bool SalvarFoto(int alunoId, IFormFile foto)
        {
            using var ms = new MemoryStream();
            foto.CopyTo(ms);
            var base64String = Convert.ToBase64String(ms.ToArray());
            return _repository.AtualizarFoto(alunoId, base64String);
        }

        public string? ObterFoto(int alunoId) => _repository.ObterFoto(alunoId);
    }
}