using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepository(DatabaseHelper dbHelper, ILogger<Reserva> logger)
            : base(dbHelper, logger)
        {
        }

        public override async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las reservas");
            var reservas = new List<Reserva>();
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} reservas", reservas.Count);
            return reservas;
        }

        public override async Task<Reserva?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando reserva por ID: {Id}", id);
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva WHERE id_reserva = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    };
                }
                _logger.LogWarning("Reserva con ID: {Id} no encontrada", id);
                return null;
            }
        }

        public override async Task<int> CreateAsync(Reserva reserva)
        {
            _logger.LogInformation("Creando nueva reserva - Inmueble: {InmuebleId}, Inquilino: {InquilinoId}",
                reserva.IdInmueble, reserva.IdInquilino);
            var query = @"INSERT INTO reserva 
                        (fecha_inicio, fecha_fin, fecha_fin_original, monto_por_dia, 
                         estado, fecha_creacion, id_inquilino, id_inmueble, 
                         id_usuario_creador) 
                        VALUES (@FechaInicio, @FechaFin, @FechaFinOriginal, @MontoPorDia, 
                                @Estado, @FechaCreacion, @IdInquilino, @IdInmueble, 
                                @IdUsuarioCreador);
                        SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@FechaInicio", reserva.FechaInicio),
                new MySqlParameter("@FechaFin", reserva.FechaFin),
                new MySqlParameter("@FechaFinOriginal", reserva.FechaFinOriginal ?? (object)DBNull.Value),
                new MySqlParameter("@MontoPorDia", reserva.MontoPorDia),
                new MySqlParameter("@Estado", reserva.Estado ?? "Activa"),
                new MySqlParameter("@FechaCreacion", reserva.FechaCreacion == default ? DateTime.Now : reserva.FechaCreacion),
                new MySqlParameter("@IdInquilino", reserva.IdInquilino),
                new MySqlParameter("@IdInmueble", reserva.IdInmueble),
                new MySqlParameter("@IdUsuarioCreador", reserva.IdUsuarioCreador)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var id = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Reserva creada con ID: {Id}", id);
            return id;
        }

        public override async Task UpdateAsync(Reserva reserva)
        {
            _logger.LogInformation("Actualizando reserva ID: {Id}", reserva.Id);
            var query = @"UPDATE reserva 
                        SET fecha_inicio = @FechaInicio, 
                            fecha_fin = @FechaFin, 
                            fecha_fin_original = @FechaFinOriginal,
                            monto_por_dia = @MontoPorDia, 
                            estado = @Estado, 
                            id_inquilino = @IdInquilino, 
                            id_inmueble = @IdInmueble 
                        WHERE id_reserva = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", reserva.Id),
                new MySqlParameter("@FechaInicio", reserva.FechaInicio),
                new MySqlParameter("@FechaFin", reserva.FechaFin),
                new MySqlParameter("@FechaFinOriginal", reserva.FechaFinOriginal ?? (object)DBNull.Value),
                new MySqlParameter("@MontoPorDia", reserva.MontoPorDia),
                new MySqlParameter("@Estado", reserva.Estado),
                new MySqlParameter("@IdInquilino", reserva.IdInquilino),
                new MySqlParameter("@IdInmueble", reserva.IdInmueble)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Reserva ID: {Id} actualizada correctamente", reserva.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando reserva ID: {Id}", id);
            var query = "DELETE FROM reserva WHERE id_reserva = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Reserva ID: {Id} eliminada correctamente", id);
        }

        // obtiene reservas vigentes
        public async Task<IEnumerable<Reserva>> GetVigentesAsync()
        {
            _logger.LogInformation("Obteniendo reservas vigentes");
            var hoy = DateTime.Now.Date;
            var reservas = new List<Reserva>();
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva 
                          WHERE estado = 'Activa' AND fecha_inicio <= @Hoy AND fecha_fin >= @Hoy";

            var parameters = new MySqlParameter[] { new MySqlParameter("@Hoy", hoy) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} reservas vigentes", reservas.Count);
            return reservas;
        }

        // obtiene reservas que terminan en X dias
        public async Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias)
        {
            _logger.LogInformation("Obteniendo reservas que terminan en {Dias} dias", dias);
            var fechaLimite = DateTime.Now.Date.AddDays(dias);
            var reservas = new List<Reserva>();
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva 
                          WHERE estado = 'Activa' AND fecha_fin = @FechaLimite";

            var parameters = new MySqlParameter[] { new MySqlParameter("@FechaLimite", fechaLimite) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} reservas que terminan en {Dias} dias", reservas.Count, dias);
            return reservas;
        }

        // obtiene reservas de un inmueble
        public async Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId)
        {
            _logger.LogInformation("Obteniendo reservas del inmueble {InmuebleId}", inmuebleId);
            var reservas = new List<Reserva>();
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva WHERE id_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", inmuebleId) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} reservas para el inmueble {InmuebleId}", reservas.Count, inmuebleId);
            return reservas;
        }

        // obtiene reservas de un inquilino
        public async Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId)
        {
            _logger.LogInformation("Obteniendo reservas del inquilino {InquilinoId}", inquilinoId);
            var reservas = new List<Reserva>();
            var query = @"SELECT id_reserva, fecha_inicio, fecha_fin, fecha_fin_original, 
                                 monto_por_dia, estado, fecha_creacion, fecha_terminacion, 
                                 multa_aplicada, id_inquilino, id_inmueble, 
                                 id_usuario_creador, id_usuario_terminacion 
                          FROM reserva WHERE id_inquilino = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", inquilinoId) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(0),
                        FechaInicio = reader.GetDateTime(1),
                        FechaFin = reader.GetDateTime(2),
                        FechaFinOriginal = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3),
                        MontoPorDia = reader.GetDecimal(4),
                        Estado = reader.GetString(5),
                        FechaCreacion = reader.GetDateTime(6),
                        FechaTerminacion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        MultaAplicada = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        IdInquilino = reader.GetInt32(9),
                        IdInmueble = reader.GetInt32(10),
                        IdUsuarioCreador = reader.GetInt32(11),
                        IdUsuarioTerminacion = reader.IsDBNull(12) ? (int?)null : reader.GetInt32(12)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} reservas para el inquilino {InquilinoId}", reservas.Count, inquilinoId);
            return reservas;
        }

        // verifica si un inmueble esta ocupado entre dos fechas
        public async Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId} entre {Inicio} y {Fin}",
                inmuebleId, inicio, fin);
            var query = @"SELECT COUNT(1) FROM reserva 
                          WHERE id_inmueble = @Id 
                          AND estado = 'Activa'
                          AND fecha_inicio < @Fin 
                          AND fecha_fin > @Inicio";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", inmuebleId),
                new MySqlParameter("@Inicio", inicio),
                new MySqlParameter("@Fin", fin)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Inmueble {InmuebleId} ocupado: {Ocupado}", inmuebleId, count > 0);
            return count > 0;
        }

        // finaliza una reserva
        public async Task FinalizarAsync(int id, DateTime fechaTerminacion, decimal? multa, int idUsuarioTerminacion)
        {
            _logger.LogInformation("Finalizando reserva {Id} con multa de {Multa}", id, multa);
            var query = @"UPDATE reserva 
                        SET estado = 'Finalizada', 
                            fecha_terminacion = @FechaTerminacion, 
                            multa_aplicada = @Multa,
                            id_usuario_terminacion = @IdUsuarioTerminacion 
                        WHERE id_reserva = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", id),
                new MySqlParameter("@FechaTerminacion", fechaTerminacion),
                new MySqlParameter("@Multa", multa ?? (object)DBNull.Value),
                new MySqlParameter("@IdUsuarioTerminacion", idUsuarioTerminacion)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Reserva {Id} finalizada correctamente", id);
        }

        // renueva una reserva
        public async Task<Reserva> RenovarAsync(Reserva nuevaReserva)
        {
            _logger.LogInformation("Renovando reserva - Inmueble: {InmuebleId}", nuevaReserva.IdInmueble);
            var id = await CreateAsync(nuevaReserva);
            nuevaReserva.Id = id;
            return nuevaReserva;
        }
    }
}
