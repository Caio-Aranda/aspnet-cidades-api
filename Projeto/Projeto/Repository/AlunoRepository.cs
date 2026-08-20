using MySql.Data.MySqlClient;
using Projeto.Entidades;
using Projeto.Repository;

namespace Projeto.Repository
{
    public class AlunoRepository
    {
        private readonly MySqlDbContext _context;

        public AlunoRepository(MySqlDbContext context)
        {
            _context = context;
        }

        public bool Criar(Aluno aluno)
        {
            bool sucesso = false;
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = @"insert into Aluno(Nome, Idade, CidadeId) 
                                        values (@Nome, @Idade, @CidadeId)";

                    cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                    cmd.Parameters.AddWithValue("@Idade", aluno.Idade);
                    cmd.Parameters.AddWithValue("@CidadeId", aluno.Cidade.CidadeId);

                    cmd.ExecuteNonQuery();
                    aluno.Id = (int)cmd.LastInsertedId;
                    sucesso = true;
                }
            }
            catch (MySqlException) { throw; }
            return sucesso;
        }

        public bool Alterar(Aluno aluno)
        {
            bool sucesso = false;
            MySqlTransaction? transacao = null;
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    transacao = _context.GetConexao().BeginTransaction();

                    // Histórico
                    cmd.CommandText = "select * from Aluno where AlunoId = @AlunoId";
                    cmd.Parameters.AddWithValue("@AlunoId", aluno.Id);
                    var dr = cmd.ExecuteReader();

                    // Simulação da tabela de histórico que estava no seu código
                    if (dr.Read())
                    {
                        var idAntigo = dr.GetInt32("AlunoId");
                        var nomeAntigo = dr.GetString("Nome");
                        var idadeAntiga = dr.GetInt32("Idade");
                        var cidadeIdAntiga = dr.GetInt32("CidadeId");
                        dr.Close();

                        cmd.CommandText = @"insert into AlunoHistorico(AlunoId, Nome, Idade, CidadeId, DataAlteracao) 
                                            values (@HAlunoId, @HNome, @HIdade, @HCidadeId, @HData) ";

                        cmd.Parameters.AddWithValue("@HAlunoId", idAntigo);
                        cmd.Parameters.AddWithValue("@HNome", nomeAntigo);
                        cmd.Parameters.AddWithValue("@HIdade", idadeAntiga);
                        cmd.Parameters.AddWithValue("@HCidadeId", cidadeIdAntiga);
                        cmd.Parameters.AddWithValue("@HData", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        dr.Close();
                    }

                    // Atualização
                    cmd.Parameters.Clear();
                    cmd.CommandText = @"update Aluno 
                                        set Nome = @Nome, Idade = @Idade, CidadeId = @CidadeId 
                                        where AlunoId = @AlunoId";

                    cmd.Parameters.AddWithValue("@Nome", aluno.Nome);
                    cmd.Parameters.AddWithValue("@Idade", aluno.Idade);
                    cmd.Parameters.AddWithValue("@CidadeId", aluno.Cidade.CidadeId);
                    cmd.Parameters.AddWithValue("@AlunoId", aluno.Id);

                    cmd.ExecuteNonQuery();
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

        public void Excluir(int id)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "delete from Aluno where AlunoId = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException) { throw; }
        }

        public Aluno? Obter(int id)
        {
            Aluno? aluno = null;
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Aluno where AlunoId = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);
                    using var dr = cmd.ExecuteReader();
                    if (dr.Read()) aluno = Map(dr);
                }
            }
            catch (MySqlException) { throw; }
            return aluno;
        }

        public IEnumerable<Aluno> ObterTodos()
        {
            List<Aluno> alunos = new();
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Aluno";
                    using var dr = cmd.ExecuteReader();
                    while (dr.Read()) alunos.Add(Map(dr));
                }
            }
            catch (MySqlException) { throw; }
            return alunos;
        }

        public IEnumerable<Aluno> Consultar(string nome)
        {
            List<Aluno> alunos = new();
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select * from Aluno where Nome like @Nome";
                    cmd.Parameters.AddWithValue("@Nome", "%" + nome + "%");
                    using var dr = cmd.ExecuteReader();
                    while (dr.Read()) alunos.Add(Map(dr));
                }
            }
            catch (MySqlException) { throw; }
            return alunos;
        }

        public int TotalAlunos()
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select count(*) from Aluno";
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (MySqlException) { throw; }
        }

        public bool AlunoExistente(string nome)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select count(*) from Aluno where Nome = @Nome";
                    cmd.Parameters.AddWithValue("@Nome", nome);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (MySqlException) { throw; }
        }

        public bool AlunoExistente(int id)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select count(*) from Aluno where AlunoId = @AlunoId";
                    cmd.Parameters.AddWithValue("@AlunoId", id);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
            catch (MySqlException) { throw; }
        }

        public bool AtualizarFoto(int id, string fotoBase64)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "update Aluno set FotoBase64 = @FotoBase64 where AlunoId = @AlunoId";
                    cmd.Parameters.AddWithValue("@FotoBase64", fotoBase64);
                    cmd.Parameters.AddWithValue("@AlunoId", id);
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (MySqlException) { throw; }
        }

        public string? ObterFoto(int id)
        {
            try
            {
                using (var cmd = _context.GetConexao().CreateCommand())
                {
                    cmd.CommandText = "select FotoBase64 from Aluno where AlunoId = @AlunoId";
                    cmd.Parameters.AddWithValue("@AlunoId", id);
                    var aux = cmd.ExecuteScalar();
                    if (aux != DBNull.Value && aux != null) return aux.ToString();
                }
            }
            catch (MySqlException) { throw; }
            return null;
        }

        private Aluno Map(MySqlDataReader dr)
        {
            return new Aluno
            {
                Id = dr.GetInt32("AlunoId"),
                Nome = dr.GetString("Nome"),
                Idade = dr.GetInt32("Idade"),
                Cidade = new Cidade { CidadeId = dr.GetInt32("CidadeId") }
            };
        }
    }
}