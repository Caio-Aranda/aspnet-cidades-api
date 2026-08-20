namespace Projeto.Controllers.DTOS
{
    public class AlunoCriarRequest
    {
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public int CidadeId { get; set; }
    }
}