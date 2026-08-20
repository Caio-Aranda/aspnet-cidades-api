using MySql.Data.MySqlClient;

namespace Projeto.Repository
{
    public class MySqlDbContext : IDisposable
    {
        private readonly MySqlConnection _conexao;

        public MySqlDbContext(IConfiguration configuration)
        {
            _conexao = new MySqlConnection(configuration.GetConnectionString("DefaultConnection"));
            _conexao.Open(); 
        }

        public MySqlConnection GetConexao()
        {
            return _conexao;
        }

        public void Dispose()
        {
            if (_conexao != null && _conexao.State == System.Data.ConnectionState.Open)
            {
                _conexao.Close();
                _conexao.Dispose();
            }
        }
    }
}