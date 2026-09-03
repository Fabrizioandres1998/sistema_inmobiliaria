using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public class InmuebleRepository : BaseRepository<Inmueble>, IInmuebleRepository
    {
        public InmuebleRepository(DatabaseHelper dbHelper, ILogger<Inmueble> logger)
            : base(dbHelper, logger)
        {
        }

        public override async Task<IEnumerable<Inmueble>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los inmuebles");
            var inmuebles = new List<Inmueble>();
            var query = @"SELECT id_inmueble, direccion, cupo_maximo, coordenadas, 
                                precio_por_dia, imagen_portada, disponible, 
                                porcentaje_reserva, fecha_creacion, 
                                id_propietario, id_tipo_inmueble 
                        FROM inmueble";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} inmuebles", inmuebles.Count);
            return inmuebles;
        }

        public override async Task<Inmueble?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando inmueble por ID: {Id}", id);
            var query = @"SELECT id_inmueble, direccion, cupo_maximo, coordenadas, 
                                precio_por_dia, imagen_portada, disponible, 
                                porcentaje_reserva, fecha_creacion, 
                                id_propietario, id_tipo_inmueble 
                        FROM inmueble 
                        WHERE id_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                if (await reader.ReadAsync())
                {
                    return new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    };
                }
                _logger.LogWarning("Inmueble con ID: {Id} no encontrado", id);
                return null;
            }
        }

        public override async Task<int> CreateAsync(Inmueble inmueble)
        {
            _logger.LogInformation("Creando nuevo inmueble - Direccion: {Direccion}", inmueble.Direccion);
            var query = @"INSERT INTO inmueble 
                        (direccion, cupo_maximo, coordenadas, precio_por_dia, 
                         imagen_portada, disponible, porcentaje_reserva, 
                         fecha_creacion, id_propietario, id_tipo_inmueble) 
                        VALUES (@Direccion, @CupoMaximo, @Coordenadas, @PrecioPorDia, 
                                @ImagenPortada, @Disponible, @PorcentajeReserva, 
                                @FechaCreacion, @IdPropietario, @IdTipoInmueble);
                        SELECT LAST_INSERT_ID();";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Direccion", inmueble.Direccion),
                new MySqlParameter("@CupoMaximo", inmueble.CupoMaximo),
                new MySqlParameter("@Coordenadas", string.IsNullOrEmpty(inmueble.Coordenadas) ? (object)DBNull.Value : inmueble.Coordenadas),
                new MySqlParameter("@PrecioPorDia", inmueble.PrecioPorDia),
                new MySqlParameter("@ImagenPortada", string.IsNullOrEmpty(inmueble.ImagenPortada) ? (object)DBNull.Value : inmueble.ImagenPortada),
                new MySqlParameter("@Disponible", inmueble.Disponible),
                new MySqlParameter("@PorcentajeReserva", inmueble.PorcentajeReserva),
                new MySqlParameter("@FechaCreacion", inmueble.FechaCreacion),
                new MySqlParameter("@IdPropietario", inmueble.IdPropietario),
                new MySqlParameter("@IdTipoInmueble", inmueble.IdTipoInmueble)
            };

            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var id = result != null ? Convert.ToInt32(result) : 0;
            _logger.LogInformation("Inmueble creado con ID: {Id}", id);
            return id;
        }

        public override async Task UpdateAsync(Inmueble inmueble)
        {
            _logger.LogInformation("Actualizando inmueble ID: {Id}", inmueble.Id);
            var query = @"UPDATE inmueble 
                        SET direccion = @Direccion, 
                            cupo_maximo = @CupoMaximo, 
                            coordenadas = @Coordenadas, 
                            precio_por_dia = @PrecioPorDia, 
                            imagen_portada = @ImagenPortada, 
                            disponible = @Disponible, 
                            porcentaje_reserva = @PorcentajeReserva, 
                            id_propietario = @IdPropietario, 
                            id_tipo_inmueble = @IdTipoInmueble 
                        WHERE id_inmueble = @Id";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", inmueble.Id),
                new MySqlParameter("@Direccion", inmueble.Direccion),
                new MySqlParameter("@CupoMaximo", inmueble.CupoMaximo),
                new MySqlParameter("@Coordenadas", string.IsNullOrEmpty(inmueble.Coordenadas) ? (object)DBNull.Value : inmueble.Coordenadas),
                new MySqlParameter("@PrecioPorDia", inmueble.PrecioPorDia),
                new MySqlParameter("@ImagenPortada", string.IsNullOrEmpty(inmueble.ImagenPortada) ? (object)DBNull.Value : inmueble.ImagenPortada),
                new MySqlParameter("@Disponible", inmueble.Disponible),
                new MySqlParameter("@PorcentajeReserva", inmueble.PorcentajeReserva),
                new MySqlParameter("@IdPropietario", inmueble.IdPropietario),
                new MySqlParameter("@IdTipoInmueble", inmueble.IdTipoInmueble)
            };

            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Inmueble ID: {Id} actualizado correctamente", inmueble.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando inmueble ID: {Id}", id);
            var query = "DELETE FROM inmueble WHERE id_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Inmueble ID: {Id} eliminado correctamente", id);
        }

        // obtiene inmuebles de un propietario
        public async Task<IEnumerable<Inmueble>> GetByPropietarioIdAsync(int propietarioId)
        {
            _logger.LogInformation("Obteniendo inmuebles del propietario ID: {PropietarioId}", propietarioId);
            var inmuebles = new List<Inmueble>();
            var query = @"SELECT id_inmueble, direccion, cupo_maximo, coordenadas, 
                                 precio_por_dia, imagen_portada, disponible, 
                                 porcentaje_reserva, fecha_creacion, 
                                 id_propietario, id_tipo_inmueble 
                          FROM inmueble 
                          WHERE id_propietario = @PropietarioId";

            var parameters = new MySqlParameter[] { new MySqlParameter("@PropietarioId", propietarioId) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} inmuebles para el propietario {PropietarioId}", inmuebles.Count, propietarioId);
            return inmuebles;
        }

        // solo inmuebles disponibles
        public async Task<IEnumerable<Inmueble>> GetDisponiblesAsync()
        {
            _logger.LogInformation("Obteniendo inmuebles disponibles");
            var inmuebles = new List<Inmueble>();
            var query = @"SELECT id_inmueble, direccion, cupo_maximo, coordenadas, 
                                 precio_por_dia, imagen_portada, disponible, 
                                 porcentaje_reserva, fecha_creacion, 
                                 id_propietario, id_tipo_inmueble 
                          FROM inmueble 
                          WHERE disponible = true";

            using (var reader = await _dbHelper.ExecuteReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} inmuebles disponibles", inmuebles.Count);
            return inmuebles;
        }

        // inmuebles disponibles en un rango de fechas
        public async Task<IEnumerable<Inmueble>> GetDisponiblesEnFechasAsync(DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Buscando inmuebles disponibles entre {Inicio} y {Fin}", inicio, fin);
            var inmuebles = new List<Inmueble>();
            var query = @"
                SELECT i.id_inmueble, i.direccion, i.cupo_maximo, i.coordenadas, 
                       i.precio_por_dia, i.imagen_portada, i.disponible, 
                       i.porcentaje_reserva, i.fecha_creacion, 
                       i.id_propietario, i.id_tipo_inmueble 
                FROM inmueble i
                WHERE i.disponible = true
                AND NOT EXISTS (
                    SELECT 1 FROM reserva r 
                    WHERE r.id_inmueble = i.id_inmueble
                    AND r.fecha_inicio < @Fin 
                    AND r.fecha_fin > @Inicio
                )";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Inicio", inicio),
                new MySqlParameter("@Fin", fin)
            };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se encontraron {Count} inmuebles disponibles en esas fechas", inmuebles.Count);
            return inmuebles;
        }

        // inmuebles mas reservados (informe)
        public async Task<IEnumerable<Inmueble>> GetMasReservadosAsync(int dias)
        {
            _logger.LogInformation("Obteniendo inmuebles mas reservados en los ultimos {Dias} dias", dias);
            var fechaLimite = DateTime.Now.AddDays(-dias);
            var inmuebles = new List<Inmueble>();
            var query = @"
                SELECT i.id_inmueble, i.direccion, i.cupo_maximo, i.coordenadas, 
                       i.precio_por_dia, i.imagen_portada, i.disponible, 
                       i.porcentaje_reserva, i.fecha_creacion, 
                       i.id_propietario, i.id_tipo_inmueble,
                       COUNT(r.id_reserva) as CantidadReservas
                FROM inmueble i
                INNER JOIN reserva r ON i.id_inmueble = r.id_inmueble
                WHERE r.fecha_creacion >= @FechaLimite
                GROUP BY i.id_inmueble
                ORDER BY CantidadReservas DESC";

            var parameters = new MySqlParameter[] { new MySqlParameter("@FechaLimite", fechaLimite) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} inmuebles mas reservados", inmuebles.Count);
            return inmuebles;
        }

        // inmuebles sin reservas (informe)
        public async Task<IEnumerable<Inmueble>> GetSinReservasAsync(int dias)
        {
            _logger.LogInformation("Obteniendo inmuebles sin reservas en los ultimos {Dias} dias", dias);
            var fechaLimite = DateTime.Now.AddDays(-dias);
            var inmuebles = new List<Inmueble>();
            var query = @"
                SELECT i.id_inmueble, i.direccion, i.cupo_maximo, i.coordenadas, 
                    i.precio_por_dia, i.imagen_portada, i.disponible, 
                    i.porcentaje_reserva, i.fecha_creacion, 
                    i.id_propietario, i.id_tipo_inmueble 
                FROM inmueble i
                LEFT JOIN reserva r ON i.id_inmueble = r.id_inmueble 
                    AND r.fecha_creacion >= @FechaLimite
                WHERE r.id_reserva IS NULL";

            var parameters = new MySqlParameter[] { new MySqlParameter("@FechaLimite", fechaLimite) };

            using (var reader = await _dbHelper.ExecuteReaderAsync(query, parameters))
            {
                while (await reader.ReadAsync())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(0),
                        Direccion = reader.GetString(1),
                        CupoMaximo = reader.GetInt32(2),
                        Coordenadas = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                        PrecioPorDia = reader.GetDecimal(4),
                        ImagenPortada = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                        Disponible = reader.GetBoolean(6),
                        PorcentajeReserva = reader.GetInt32(7),
                        FechaCreacion = reader.GetDateTime(8),
                        IdPropietario = reader.GetInt32(9),
                        IdTipoInmueble = reader.GetInt32(10)
                    });
                }
            }
            _logger.LogInformation("Se obtuvieron {Count} inmuebles sin reservas", inmuebles.Count);
            return inmuebles;
        }

        // verifica disponibilidad en fechas para una reserva
        public async Task<bool> EstaDisponibleEnFechasAsync(int inmuebleId, DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId} entre {Inicio} y {Fin}", inmuebleId, inicio, fin);
            var query = @"
                SELECT COUNT(1) FROM reserva 
                WHERE id_inmueble = @Id
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
            var disponible = count == 0;
            _logger.LogInformation("Inmueble {InmuebleId} disponible: {Disponible}", inmuebleId, disponible);
            return disponible;
        }

        // valida que no exista la misma direccion
        public async Task<bool> ExisteDireccionAsync(string direccion)
        {
            _logger.LogInformation("Verificando si existe direccion: {Direccion}", direccion);
            var query = "SELECT COUNT(1) FROM inmueble WHERE direccion = @Direccion";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Direccion", direccion) };
            var result = await _dbHelper.ExecuteScalarAsync(query, parameters);
            var count = result != null ? Convert.ToInt32(result) : 0;
            return count > 0;
        }

        // suspende inmueble (no aparece en listados)
        public async Task SuspenderAsync(int id)
        {
            _logger.LogInformation("Suspendiendo inmueble ID: {Id}", id);
            var query = "UPDATE inmueble SET disponible = false WHERE id_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Inmueble ID: {Id} suspendido", id);
        }

        // reactiva inmueble
        public async Task ActivarAsync(int id)
        {
            _logger.LogInformation("Activando inmueble ID: {Id}", id);
            var query = "UPDATE inmueble SET disponible = true WHERE id_inmueble = @Id";
            var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
            await _dbHelper.ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Inmueble ID: {Id} activado", id);
        }
    }
}