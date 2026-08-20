using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;

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
            var query = "SELECT id_inquilino, nombre_completo, dni, email, telefono, direccion, fecha_registro FROM inquilino";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    inquilinos.Add(new Inquilino
                    {
                        Id = reader.GetInt32(0),
                        NombreCompleto = reader.GetString(1),
                        Dni = reader.GetString(2),
                        Email = reader.GetString(3),
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
            var query = "SELECT id_inquilino, nombre_completo, dni, email, telefono, direccion, fecha_registro FROM inquilino WHERE id_inquilino = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Inquilino
                    {
                        Id = reader.GetInt32(0),
                        NombreCompleto = reader.GetString(1),
                        Dni = reader.GetString(2),
                        Email = reader.GetString(3),
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
            var query = @"INSERT INTO inquilino (nombre_completo, dni, email, telefono, direccion, fecha_registro) 
                          VALUES (@NombreCompleto, @Dni, @Email, @Telefono, @Direccion, @FechaRegistro);
                          SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@NombreCompleto", inquilino.NombreCompleto),
                new MySqlParameter("@Dni", inquilino.Dni),
                new MySqlParameter("@Email", inquilino.Email),
                new MySqlParameter("@Telefono", string.IsNullOrEmpty(inquilino.Telefono) ? (object)DBNull.Value : inquilino.Telefono),
                new MySqlParameter("@Direccion", string.IsNullOrEmpty(inquilino.Direccion) ? (object)DBNull.Value : inquilino.Direccion),
                new MySqlParameter("@FechaRegistro", inquilino.FechaRegistro)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task UpdateAsync(Inquilino inquilino)
        {
            var query = @"UPDATE inquilino 
                          SET nombre_completo = @NombreCompleto, 
                              dni = @Dni, 
                              email = @Email, 
                              telefono = @Telefono, 
                              direccion = @Direccion 
                          WHERE id_inquilino = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", inquilino.Id),
                new MySqlParameter("@NombreCompleto", inquilino.NombreCompleto),
                new MySqlParameter("@Dni", inquilino.Dni),
                new MySqlParameter("@Email", inquilino.Email),
                new MySqlParameter("@Telefono", string.IsNullOrEmpty(inquilino.Telefono) ? (object)DBNull.Value : inquilino.Telefono),
                new MySqlParameter("@Direccion", string.IsNullOrEmpty(inquilino.Direccion) ? (object)DBNull.Value : inquilino.Direccion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM inquilino WHERE id_inquilino = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            var query = "SELECT COUNT(1) FROM inquilino WHERE dni = @Dni";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Dni", dni) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }
    }
}