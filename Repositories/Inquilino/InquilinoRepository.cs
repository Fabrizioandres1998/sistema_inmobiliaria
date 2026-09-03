using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public class InquilinoRepository : BaseRepository<Inquilino>, IInquilinoRepository
    {
        public InquilinoRepository(DatabaseHelper dbHelper, ILogger<Inquilino> logger)
            : base(dbHelper, logger)
        {
        }

        public override async Task<IEnumerable<Inquilino>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los inquilinos");
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
            _logger.LogInformation("Se obtuvieron {Count} inquilinos", inquilinos.Count);
            return inquilinos;
        }

        public override async Task<Inquilino?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando inquilino por ID: {Id}", id);
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
                _logger.LogWarning("Inquilino con ID: {Id} no encontrado", id);
                return null;
            }
        }

        public override async Task<int> CreateAsync(Inquilino inquilino)
        {
            _logger.LogInformation("Creando nuevo inquilino - Nombre: {Nombre}, DNI: {Dni}", inquilino.NombreCompleto, inquilino.Dni);
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
            var id = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Inquilino creado con ID: {Id}", id);
            return id;
        }

        public override async Task UpdateAsync(Inquilino inquilino)
        {
            _logger.LogInformation("Actualizando inquilino ID: {Id}", inquilino.Id);
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
            _logger.LogInformation("Inquilino ID: {Id} actualizado correctamente", inquilino.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando inquilino ID: {Id}", id);
            var query = "DELETE FROM inquilino WHERE id_inquilino = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Inquilino ID: {Id} eliminado correctamente", id);
        }

        // verifica si el dni ya existe
        public async Task<bool> ExisteDniAsync(string dni)
        {
            _logger.LogInformation("Verificando si existe DNI: {Dni}", dni);
            var query = "SELECT COUNT(1) FROM inquilino WHERE dni = @Dni";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Dni", dni) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("DNI: {Dni} existe: {Existe}", dni, count > 0);
            return count > 0;
        }
    }
}