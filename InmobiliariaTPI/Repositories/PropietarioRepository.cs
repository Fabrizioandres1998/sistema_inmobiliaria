using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using Microsoft.Data.SqlClient;

namespace InmobiliariaTPI.Repositories
{
    public class PropietarioRepository : IPropietarioRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public PropietarioRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public async Task<IEnumerable<Propietario>> GetAllAsync()
        {
            var propietarios = new List<Propietario>();
            var query = "SELECT Id, NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro FROM Propietarios";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    propietarios.Add(new Propietario
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
            return propietarios;
        }

        public async Task<Propietario?> GetByIdAsync(int id)
        {
            var query = "SELECT Id, NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro FROM Propietarios WHERE Id = @Id";
            var parameters = new SqlParameter[] { new SqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Propietario
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

        public async Task<int> CreateAsync(Propietario propietario)
        {
            var query = @"INSERT INTO Propietarios (NombreCompleto, Dni, Email, Telefono, Direccion, FechaRegistro) 
                        VALUES (@NombreCompleto, @Dni, @Email, @Telefono, @Direccion, @FechaRegistro);
                        SELECT SCOPE_IDENTITY();";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@NombreCompleto", propietario.NombreCompleto),
                new SqlParameter("@Dni", propietario.Dni),
                new SqlParameter("@Email", string.IsNullOrEmpty(propietario.Email) ? (object)DBNull.Value : propietario.Email),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(propietario.Telefono) ? (object)DBNull.Value : propietario.Telefono),
                new SqlParameter("@Direccion", string.IsNullOrEmpty(propietario.Direccion) ? (object)DBNull.Value : propietario.Direccion),
                new SqlParameter("@FechaRegistro", propietario.FechaRegistro)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task UpdateAsync(Propietario propietario)
        {
            var query = @"UPDATE Propietarios 
                        SET NombreCompleto = @NombreCompleto, 
                            Dni = @Dni, 
                            Email = @Email, 
                            Telefono = @Telefono, 
                            Direccion = @Direccion 
                            WHERE Id = @Id";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", propietario.Id),
                new SqlParameter("@NombreCompleto", propietario.NombreCompleto),
                new SqlParameter("@Dni", propietario.Dni),
                new SqlParameter("@Email", string.IsNullOrEmpty(propietario.Email) ? (object)DBNull.Value : propietario.Email),
                new SqlParameter("@Telefono", string.IsNullOrEmpty(propietario.Telefono) ? (object)DBNull.Value : propietario.Telefono),
                new SqlParameter("@Direccion", string.IsNullOrEmpty(propietario.Direccion) ? (object)DBNull.Value : propietario.Direccion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Propietarios WHERE Id = @Id";
            var parameters = new SqlParameter[] { new SqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            var query = "SELECT COUNT(1) FROM Propietarios WHERE Dni = @Dni";
            var parameters = new SqlParameter[] { new SqlParameter("@Dni", dni) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }
    }
}
