using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public class TipoInmuebleRepository : BaseRepository<TipoInmueble>, ITipoInmuebleRepository
    {
        public TipoInmuebleRepository(DatabaseHelper dbHelper, ILogger<TipoInmueble> logger)
            : base(dbHelper, logger)
        {
        }

        public override async Task<IEnumerable<TipoInmueble>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los tipos de inmueble");
            var tipos = new List<TipoInmueble>();
            var query = "SELECT id_tipo_inmueble, nombre, descripcion FROM tipo_inmueble";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    tipos.Add(new TipoInmueble
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} tipos de inmueble", tipos.Count);
            return tipos;
        }

        public override async Task<TipoInmueble?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando tipo de inmueble por ID: {Id}", id);
            var query = "SELECT id_tipo_inmueble, nombre, descripcion FROM tipo_inmueble WHERE id_tipo_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new TipoInmueble
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                    };
                }
                _logger.LogWarning("Tipo de inmueble con ID: {Id} no encontrado", id);
                return null;
            }
        }

        public override async Task<int> CreateAsync(TipoInmueble tipo)
        {
            _logger.LogInformation("Creando nuevo tipo de inmueble - Nombre: {Nombre}", tipo.Nombre);
            var query = @"INSERT INTO tipo_inmueble (nombre, descripcion) 
                        VALUES (@Nombre, @Descripcion);
                        SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Nombre", tipo.Nombre),
                new MySqlParameter("@Descripcion", string.IsNullOrEmpty(tipo.Descripcion) ? (object)DBNull.Value : tipo.Descripcion)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var id = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Tipo de inmueble creado con ID: {Id}", id);
            return id;
        }

        public override async Task UpdateAsync(TipoInmueble tipo)
        {
            _logger.LogInformation("Actualizando tipo de inmueble ID: {Id}", tipo.Id);
            var query = @"UPDATE tipo_inmueble 
                        SET nombre = @Nombre, 
                            descripcion = @Descripcion 
                        WHERE id_tipo_inmueble = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", tipo.Id),
                new MySqlParameter("@Nombre", tipo.Nombre),
                new MySqlParameter("@Descripcion", string.IsNullOrEmpty(tipo.Descripcion) ? (object)DBNull.Value : tipo.Descripcion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Tipo de inmueble ID: {Id} actualizado correctamente", tipo.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando tipo de inmueble ID: {Id}", id);
            var query = "DELETE FROM tipo_inmueble WHERE id_tipo_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Tipo de inmueble ID: {Id} eliminado correctamente", id);
        }
    }
}