using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(DatabaseHelper dbHelper, ILogger<Pago> logger)
            : base(dbHelper, logger)
        {
        }

        public override async Task<IEnumerable<Pago>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los pagos");
            var pagos = new List<Pago>();
            var query = @"SELECT id_pago, concepto, fecha_pago, importe, estado, 
                                 fecha_creacion, fecha_anulacion, id_reserva, 
                                 id_usuario_creador, id_usuario_anulacion 
                          FROM pago";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32(0),
                        Concepto = reader.GetString(1),
                        FechaPago = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetByte(4),
                        FechaCreacion = reader.GetDateTime(5),
                        FechaAnulacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        IdReserva = reader.GetInt32(7),
                        IdUsuarioCreador = reader.GetInt32(8),
                        IdUsuarioAnulacion = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} pagos", pagos.Count);
            return pagos;
        }

        public override async Task<Pago?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando pago por ID: {Id}", id);
            var query = @"SELECT id_pago, concepto, fecha_pago, importe, estado, 
                                 fecha_creacion, fecha_anulacion, id_reserva, 
                                 id_usuario_creador, id_usuario_anulacion 
                          FROM pago 
                          WHERE id_pago = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Pago
                    {
                        Id = reader.GetInt32(0),
                        Concepto = reader.GetString(1),
                        FechaPago = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetByte(4),
                        FechaCreacion = reader.GetDateTime(5),
                        FechaAnulacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        IdReserva = reader.GetInt32(7),
                        IdUsuarioCreador = reader.GetInt32(8),
                        IdUsuarioAnulacion = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                    };
                }
                _logger.LogWarning("Pago con ID: {Id} no encontrado", id);
                return null;
            }
        }

        public override async Task<int> CreateAsync(Pago pago)
        {
            _logger.LogInformation("Creando nuevo pago - Concepto: {Concepto}, Importe: {Importe}", pago.Concepto, pago.Importe);
            var query = @"INSERT INTO pago 
                        (concepto, fecha_pago, importe, estado, fecha_creacion, id_reserva, id_usuario_creador) 
                        VALUES (@Concepto, @FechaPago, @Importe, @Estado, @FechaCreacion, @IdReserva, @IdUsuarioCreador);
                        SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Concepto", pago.Concepto),
                new MySqlParameter("@FechaPago", pago.FechaPago),
                new MySqlParameter("@Importe", pago.Importe),
                new MySqlParameter("@Estado", pago.Estado),
                new MySqlParameter("@FechaCreacion", pago.FechaCreacion == default ? DateTime.Now : pago.FechaCreacion),
                new MySqlParameter("@IdReserva", pago.IdReserva),
                new MySqlParameter("@IdUsuarioCreador", pago.IdUsuarioCreador)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var id = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Pago creado con ID: {Id}", id);
            return id;
        }

        public override async Task UpdateAsync(Pago pago)
        {
            _logger.LogInformation("Actualizando pago ID: {Id}", pago.Id);
            var query = @"UPDATE pago 
                        SET concepto = @Concepto
                        WHERE id_pago = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", pago.Id),
                new MySqlParameter("@Concepto", pago.Concepto)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Pago ID: {Id} actualizado correctamente", pago.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            // No se elimina físicamente, se anula
            throw new InvalidOperationException("Los pagos no se eliminan, se anulan");
        }

        public async Task<IEnumerable<Pago>> GetByReservaIdAsync(int reservaId)
        {
            _logger.LogInformation("Obteniendo pagos de la reserva ID: {ReservaId}", reservaId);
            var pagos = new List<Pago>();
            var query = @"SELECT id_pago, concepto, fecha_pago, importe, estado, 
                                 fecha_creacion, fecha_anulacion, id_reserva, 
                                 id_usuario_creador, id_usuario_anulacion 
                          FROM pago 
                          WHERE id_reserva = @ReservaId
                          ORDER BY fecha_pago DESC";

            var parameters = new MySqlParameter[] { new MySqlParameter("@ReservaId", reservaId) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32(0),
                        Concepto = reader.GetString(1),
                        FechaPago = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetByte(4),
                        FechaCreacion = reader.GetDateTime(5),
                        FechaAnulacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        IdReserva = reader.GetInt32(7),
                        IdUsuarioCreador = reader.GetInt32(8),
                        IdUsuarioAnulacion = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} pagos para la reserva {ReservaId}", pagos.Count, reservaId);
            return pagos;
        }

        public async Task<IEnumerable<Pago>> GetActivosByReservaIdAsync(int reservaId)
        {
            _logger.LogInformation("Obteniendo pagos activos de la reserva ID: {ReservaId}", reservaId);
            var pagos = new List<Pago>();
            var query = @"SELECT id_pago, concepto, fecha_pago, importe, estado, 
                                 fecha_creacion, fecha_anulacion, id_reserva, 
                                 id_usuario_creador, id_usuario_anulacion 
                          FROM pago 
                          WHERE id_reserva = @ReservaId AND estado = 1
                          ORDER BY fecha_pago DESC";

            var parameters = new MySqlParameter[] { new MySqlParameter("@ReservaId", reservaId) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32(0),
                        Concepto = reader.GetString(1),
                        FechaPago = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetByte(4),
                        FechaCreacion = reader.GetDateTime(5),
                        FechaAnulacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        IdReserva = reader.GetInt32(7),
                        IdUsuarioCreador = reader.GetInt32(8),
                        IdUsuarioAnulacion = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} pagos activos para la reserva {ReservaId}", pagos.Count, reservaId);
            return pagos;
        }

        public async Task AnularAsync(int id, int idUsuarioAnulacion)
        {
            _logger.LogInformation("Anulando pago ID: {Id}", id);
            var query = @"UPDATE pago 
                        SET estado = 0, 
                            fecha_anulacion = @FechaAnulacion,
                            id_usuario_anulacion = @IdUsuarioAnulacion
                        WHERE id_pago = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id),
                new MySqlParameter("@FechaAnulacion", DateTime.Now),
                new MySqlParameter("@IdUsuarioAnulacion", idUsuarioAnulacion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Pago ID: {Id} anulado correctamente", id);
        }

        public override async Task<IEnumerable<Pago>> GetPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo pagos paginados - Pagina: {Page}, Tamano: {PageSize}", page, pageSize);

            var pagos = new List<Pago>();
            var offset = (page - 1) * pageSize;

            var query = @"SELECT id_pago, concepto, fecha_pago, importe, estado, 
                                 fecha_creacion, fecha_anulacion, id_reserva, 
                                 id_usuario_creador, id_usuario_anulacion 
                          FROM pago";
            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += " WHERE concepto LIKE @SearchTerm";
                parameters.Add(new MySqlParameter("@SearchTerm", $"%{searchTerm}%"));
            }

            query += " ORDER BY id_pago DESC LIMIT @PageSize OFFSET @Offset";
            parameters.Add(new MySqlParameter("@PageSize", pageSize));
            parameters.Add(new MySqlParameter("@Offset", offset));

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters.ToArray()))
            {
                while (await reader.ReadAsync())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32(0),
                        Concepto = reader.GetString(1),
                        FechaPago = reader.GetDateTime(2),
                        Importe = reader.GetDecimal(3),
                        Estado = reader.GetByte(4),
                        FechaCreacion = reader.GetDateTime(5),
                        FechaAnulacion = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                        IdReserva = reader.GetInt32(7),
                        IdUsuarioCreador = reader.GetInt32(8),
                        IdUsuarioAnulacion = reader.IsDBNull(9) ? (int?)null : reader.GetInt32(9)
                    });
                }
            }

            _logger.LogInformation("Se obtuvieron {Count} pagos", pagos.Count);
            return pagos;
        }

        public override async Task<int> GetTotalCountAsync(string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo total de pagos");

            var query = "SELECT COUNT(1) FROM pago";
            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += " WHERE concepto LIKE @SearchTerm";
                parameters.Add(new MySqlParameter("@SearchTerm", $"%{searchTerm}%"));
            }

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters.ToArray());
            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}