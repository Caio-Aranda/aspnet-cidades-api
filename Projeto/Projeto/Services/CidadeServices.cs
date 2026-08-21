
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

                var dados = linha.Split(',');

                if (dados.Length < 6) continue;

                cidades.Add(new Cidade
                {
                    CidadeId = string.IsNullOrWhiteSpace(dados[0]) ? 0 : int.Parse(dados[0]),
                    Nome = dados[1].Replace("\"", ""),
                    Sigla = dados[2].Replace("\"", ""),
                    IBGEMunicipio = string.IsNullOrWhiteSpace(dados[3]) ? 0 : int.Parse(dados[3]),
                    Latitude = string.IsNullOrWhiteSpace(dados[4]) ? 0 :
                               Convert.ToDecimal(dados[4].Replace(".", ","), new System.Globalization.CultureInfo("pt-BR")),
                    Longitude = string.IsNullOrWhiteSpace(dados[5]) ? 0 :
                                Convert.ToDecimal(dados[5].Replace(".", ","), new System.Globalization.CultureInfo("pt-BR"))
                });
            }

            if (cidades.Count == 0) return false;

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