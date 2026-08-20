
using Projeto.Entidades;
using Projeto.Repository;

namespace Projeto.Services
{
    public class CidadeServices
    {
        private readonly CidadeRepository _repository;

        public CidadeServices(CidadeRepository repository)
        {
            _repository = repository;
        }

        public bool ImportarCsv(IFormFile arquivo)
        {
            var cidades = new List<Cidade>();
            using var stream = new StreamReader(arquivo.OpenReadStream());
            stream.ReadLine(); // Pula o cabeçalho

            while (!stream.EndOfStream)
            {
                var linha = stream.ReadLine();
                if (string.IsNullOrWhiteSpace(linha)) continue;

                var dados = linha.Split(';'); // Troque para ',' se o arquivo for separado por vírgula

                cidades.Add(new Cidade
                {
                    CidadeId = int.Parse(dados[0]),
                    Nome = dados[1],
                    Sigla = dados[2],
                    IBGEMunicipio = int.Parse(dados[3]),
                    Latitude = Convert.ToDecimal(dados[4].Replace(".", ","), new System.Globalization.CultureInfo("pt-BR")),
                    Longitude = Convert.ToDecimal(dados[5].Replace(".", ","), new System.Globalization.CultureInfo("pt-BR"))
                });
            }
            return _repository.ImportarEmMassa(cidades);
        }

        public IEnumerable<Cidade> ObterTodas() => _repository.ObterTodas();
        public int TotalCidades() => _repository.TotalCidades();
        public Cidade? Obter(int id) => _repository.Obter(id);
        public IEnumerable<string> ObterEstados() => _repository.ObterEstados();
        public IEnumerable<Cidade> ObterPorEstado(string uf) => _repository.ObterPorEstado(uf);
        public bool Alterar(Cidade cidade) => _repository.Alterar(cidade);
        public void Excluir(int id) => _repository.Excluir(id);
    }
}