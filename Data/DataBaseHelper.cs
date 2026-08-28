using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace InmobiliariaTPI.Data
{
    public class DatabaseHelper
    {
        private readonly string? _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        //INSERT, UPDATE, DELETE
        public async Task<int> ExecuteNonQueryAsync(string query, MySqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        //siempre devuelve un solo valor, es para cualquier valor de una sola celda COUNT, SUM, LAST_INSERT_ID
        public async Task<object?> ExecuteScalarAsync(string query, MySqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }

        //SELECT para cuando quiero mostrar
        public async Task<MySqlDataReader> ExecuteReaderAsync(string query, MySqlParameter[]? parameters = null)
        {
            var conn = GetConnection();
            var cmd = new MySqlCommand(query, conn);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await conn.OpenAsync();
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
    }
}