using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
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

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<object?> ExecuteScalarAsync(string query, SqlParameter[]? parameters = null)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                await conn.OpenAsync();
                return await cmd.ExecuteScalarAsync();
            }
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, SqlParameter[]? parameters = null)
        {
            var conn = GetConnection();
            var cmd = new SqlCommand(query, conn);

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            await conn.OpenAsync();
            return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }
    }
}