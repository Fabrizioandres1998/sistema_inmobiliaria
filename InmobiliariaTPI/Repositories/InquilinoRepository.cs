using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using Microsoft.Data.SqlClient;

namespace InmobiliariaTPI.Repositories
{
    public class InquilinoRepository : IInquilinoRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public InquilinoRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<IEnumerable<Inquilino>> GetAllAsync()
        {
            var inquilinos = new List<Inquilino>();
            var query = "SELECT Id, NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro FROM Inquilinos";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    inquilinos.Add(new Inquilino
                    {
                        Id = reader.GetInt32(0),
                        NombreCompleto = reader.GetString(1),
                        Dni = reader.GetString(2),
                        Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        FechaRegistro = reader.GetDateTime(6)
                    });
                }
            }
            return inquilinos;
        }

        public async Task<Inquilino?> GetByIdAsync(int id)
        {
            var query = "SELECT Id, NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro FROM Inquilinos WHERE Id = @Id";
            var parameters = new SqlParameter[] { new SqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Inquilino
                    {
                        Id = reader.GetInt32(0),
                        NombreCompleto = reader.GetString(1),
                        Dni = reader.GetString(2),
                        Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                        Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        FechaRegistro = reader.GetDateTime(6)
                    };
                }
                return null;
            }
        }

        public async Task<int> CreateAsync(Inquilino inquilino)
        {
            var query = @"INSERT INTO Inquilinos (NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro) 
                          VALUES (@NombreCompleto, @Dni, @Email, @Telefono, @Direccion, @FechaRegistro);
                          SELECT SCOPE_IDENTITY();";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@NombreCompleto", inquilino.NombreCompleto),
                new SqlParameter("@Dni", inquilino.Dni),
                new SqlParameter("@Email", string.IsNullOrEmpty(inquilino.Email) ? (object)DBNull.Value : inquilino.Email),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(inquilino.Telefono) ? (object)DBNull.Value : inquilino.Telefono),
                new SqlParameter("@Direccion", string.IsNullOrEmpty(inquilino.Direccion) ? (object)DBNull.Value : inquilino.Direccion),
                new SqlParameter("@FechaRegistro", inquilino.FechaRegistro)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task UpdateAsync(Inquilino inquilino)
        {
            var query = @"UPDATE Inquilinos 
                          SET NombreCompleto = @NombreCompleto, 
                              Dni = @Dni, 
                              Email = @Email, 
                              Telefono = @Telefono, 
                              Direccion = @Direccion 
                          WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", inquilino.Id),
                new SqlParameter("@NombreCompleto", inquilino.NombreCompleto),
                new SqlParameter("@Dni", inquilino.Dni),
                new SqlParameter("@Email", string.IsNullOrEmpty(inquilino.Email) ? (object)DBNull.Value : inquilino.Email),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(inquilino.Telefono) ? (object)DBNull.Value : inquilino.Telefono),
                new SqlParameter("@Direccion", string.IsNullOrEmpty(inquilino.Direccion) ? (object)DBNull.Value : inquilino.Direccion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Inquilinos WHERE Id = @Id";
            var parameters = new SqlParameter[] { new SqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            var query = "SELECT COUNT(1) FROM Inquilinos WHERE Dni = @Dni";
            var parameters = new SqlParameter[] { new SqlParameter("@Dni", dni) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }
    }
}
