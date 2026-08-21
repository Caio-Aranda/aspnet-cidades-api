using Projeto.Entidades;
using MySql.Data.MySqlClient;
using Projeto.Entidades;
using Projeto.Repository;

namespace Projeto.Repository
{
    public class CidadeRepository
    {
        private readonly MySqlDbContext _context;

        public CidadeRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public bool ImportarEmMassa(List<Cidade> cidades)
        {
            bool sucesso = false;
            MySqlTransaction? transacao = null;
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    transacao = _context.GetConexao().BeginTransaction();

                    cmd.CommandText = @"insert into Cidade(CidadeId, Nome, Sigla, IBGEMunicipio, Latitude, Longitude) 
                    values (@CidadeId, @Nome, @Sigla, @IBGEMunicipio, @Latitude, @Longitude)
                    ON DUPLICATE KEY UPDATE 
                    Nome=@Nome, Sigla=@Sigla, IBGEMunicipio=@IBGEMunicipio, 
                    Latitude=@Latitude, Longitude=@Longitude";

                    foreach (var cidade in cidades)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CidadeId", cidade.CidadeId);
                        cmd.Parameters.AddWithValue("@Nome", cidade.Nome);
                        cmd.Parameters.AddWithValue("@Sigla", cidade.Sigla);
                        cmd.Parameters.AddWithValue("@IBGEMunicipio", cidade.IBGEMunicipio);
                        cmd.Parameters.AddWithValue("@Latitude", cidade.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", cidade.Longitude);
                        cmd.ExecuteNonQuery();
                    }
                    transacao.Commit();
                    sucesso = true;
                }
            }
            catch (MySqlException)
            {
                transacao?.Rollback();
                throw;
            }
            return sucesso;
        }

        public IEnumerable<Cidade> ObterTodas()
        {
            List<Cidade> cidades = new();
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Cidade";
                    using var dr = cmd.ExecuteReader();
                    while (dr.Read()) cidades.Add(Map(dr));
                }
            }
            catch (MySqlException) { throw; }
            return cidades;
        }

        public int TotalCidades()
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select count(*) from Cidade";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (MySqlException) { throw; }
        }

        public Cidade? Obter(int id)
        {
            Cidade? cidade = null;
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Cidade where CidadeId = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    using var dr = cmd.ExecuteReader();
                    if (dr.Read()) cidade = Map(dr);
                }
            }
            catch (MySqlException) { throw; }
            return cidade;
        }

        public IEnumerable<string> ObterEstados()
        {
            List<string> estados = new();
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select distinct Sigla from Cidade order by Sigla";
                    using var dr = cmd.ExecuteReader();
                    while (dr.Read()) estados.Add(dr.GetString("Sigla"));
                }
            }
            catch (MySqlException) { throw; }
            return estados;
        }

        public IEnumerable<Cidade> ObterPorEstado(string uf)
        {
            List<Cidade> cidades = new();
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Cidade where Sigla = @Uf";
                    cmd.Parameters.AddWithValue("@Uf", uf);
                    using var dr = cmd.ExecuteReader();
                    while (dr.Read()) cidades.Add(Map(dr));
                }
            }
            catch (MySqlException) { throw; }
            return cidades;
        }

        public bool Alterar(Cidade cidade)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = @"update Cidade 
                                        set Nome = @Nome, Sigla = @Sigla, IBGEMunicipio = @IBGEMunicipio, 
                                            Latitude = @Latitude, Longitude = @Longitude 
                                        where CidadeId = @CidadeId";

                    cmd.Parameters.AddWithValue("@Nome", cidade.Nome);
                    cmd.Parameters.AddWithValue("@Sigla", cidade.Sigla);
                    cmd.Parameters.AddWithValue("@IBGEMunicipio", cidade.IBGEMunicipio);
                    cmd.Parameters.AddWithValue("@Latitude", cidade.Latitude);
                    cmd.Parameters.AddWithValue("@Longitude", cidade.Longitude);
                    cmd.Parameters.AddWithValue("@CidadeId", cidade.CidadeId);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException) { throw; }
        }

        public void Excluir(int id)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "delete from Cidade where CidadeId = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException) { throw; }
        }

        private Cidade Map(MySqlDataReader dr)
        {
            return new Cidade
            {
                CidadeId = dr.GetInt32("CidadeId"),
                Nome = dr.GetString("Nome"),
                Sigla = dr.GetString("Sigla"),
                IBGEMunicipio = dr.GetInt32("IBGEMunicipio"),
                Latitude = dr.GetDecimal("Latitude"),
                Longitude = dr.GetDecimal("Longitude")
            };
        }
    }
}