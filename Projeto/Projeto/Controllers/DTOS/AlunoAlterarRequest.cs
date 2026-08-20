namespace Projeto.Controllers.DTOS
{
    public class AlunoAlterarRequest
    {
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public int CidadeId { get; set; }
    }
}
