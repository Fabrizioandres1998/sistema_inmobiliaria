using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;

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
            var query = "SELECT id_propietario, nombre_completo, dni, email, telefono, direccion, fecha_registro FROM propietario";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    propietarios.Add(new Propietario
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
            return propietarios;
        }

        public async Task<Propietario?> GetByIdAsync(int id)
        {
            var query = "SELECT id_propietario, nombre_completo, dni, email, telefono, direccion, fecha_registro FROM propietario WHERE id_propietario = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Propietario
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

        public async Task<int> CreateAsync(Propietario propietario)
        {
            var query = @"INSERT INTO propietario (nombre_completo, dni, email, telefono, direccion, fecha_registro) 
                          VALUES (@NombreCompleto, @Dni, @Email, @Telefono, @Direccion, @FechaRegistro);
                          SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@NombreCompleto", propietario.NombreCompleto),
                new MySqlParameter("@Dni", propietario.Dni),
                new MySqlParameter("@Email", propietario.Email),
                new MySqlParameter("@Telefono", string.IsNullOrEmpty(propietario.Telefono) ? (object)DBNull.Value : propietario.Telefono),
                new MySqlParameter("@Direccion", string.IsNullOrEmpty(propietario.Direccion) ? (object)DBNull.Value : propietario.Direccion),
                new MySqlParameter("@FechaRegistro", propietario.FechaRegistro)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task UpdateAsync(Propietario propietario)
        {
            var query = @"UPDATE propietario 
                          SET nombre_completo = @NombreCompleto, 
                              dni = @Dni, 
                              email = @Email, 
                              telefono = @Telefono, 
                              direccion = @Direccion 
                          WHERE id_propietario = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", propietario.Id),
                new MySqlParameter("@NombreCompleto", propietario.NombreCompleto),
                new MySqlParameter("@Dni", propietario.Dni),
                new MySqlParameter("@Email", propietario.Email),
                new MySqlParameter("@Telefono", string.IsNullOrEmpty(propietario.Telefono) ? (object)DBNull.Value : propietario.Telefono),
                new MySqlParameter("@Direccion", string.IsNullOrEmpty(propietario.Direccion) ? (object)DBNull.Value : propietario.Direccion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM propietario WHERE id_propietario = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> ExisteDniAsync(string dni)
        {
            var query = "SELECT COUNT(1) FROM propietario WHERE dni = @Dni";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Dni", dni) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }
    }
}