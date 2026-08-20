using Projeto.Entidades;

namespace Projeto.Entidades
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public Cidade Cidade { get; set; } = new Cidade();
        public string? FotoBase64 { get; set; }
    }
}