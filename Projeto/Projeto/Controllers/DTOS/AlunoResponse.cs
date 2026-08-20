namespace Projeto.Controllers.DTOS
{
    public class AlunoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public int CidadeId { get; set; }
    }
}