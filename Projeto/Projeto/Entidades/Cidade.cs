namespace Projeto.Entidades
{
    public class Cidade
    {
        public int CidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public int IBGEMunicipio { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}