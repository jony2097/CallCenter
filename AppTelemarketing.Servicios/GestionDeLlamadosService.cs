 using AppTelemarketing.CapaDatos;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AppTelemarketing.Servicios

{
    public class GestionDeLlamadosService
    {
        OperacionesBD operaciones = new OperacionesBD();
        ManejoMensajesServices servicioMensajes = new ManejoMensajesServices();

        //modificar los campos Llamado y Venta cuando tocamos Aceptar
        // Implementar el procedimiento para marcar los campos LLAMADO y VENTA
        public void MarcarLlamadoYVenta(int prospectoActual, int tipoRespuesta)
        {
            string SQLUpdate = "UPDATE Prospectos SET Llamado = 1 WHERE IdProspecto = @IdProspecto";

            //creo el diccionario
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@IdProspecto", prospectoActual }
            };

            try
            {
                int rowsAffected = operaciones.Ejecutar(SQLUpdate, parametros);
                if (rowsAffected > 0)
                {
                    var resultado = servicioMensajes.ManejarResultado(true, "Se actualizó el campo Llamado correctamente.");
                }
                else
                {
                    var resultado = servicioMensajes.ManejarResultado(false, "No se encontró el prospecto especificado.");
                }

                // Si tipoRespuesta es 4, también actualizar Venta
                if (tipoRespuesta == 4)
                {
                    string SQLUpdateVenta = "UPDATE Prospectos SET Venta = 1 WHERE IdProspecto = @IdProspecto";

                    int rowsAffectedVenta = operaciones.Ejecutar(SQLUpdateVenta, parametros);

                    if (rowsAffectedVenta > 0)
                    {
                        var resultado = servicioMensajes.ManejarResultado(true, "Se actualizó el campo Venta correctamente.");
                    }
                    else
                    {
                        var resultado = servicioMensajes.ManejarResultado(false, "No se encontró el prospecto para actualizar el campo Venta.");
                    }
                }
            }
            catch (Exception ex)
            {
                var resultado = servicioMensajes.ManejarExcepcion(ex);
            }
        }



        //Aca vamos a llenar la BBDD LlamadosProspectos
        public void LlenarLlamadosProspectos(
     int idProspectoActual,
     int codRespuesta,
     DateTime fechaBotonAceptar,
     DateTime horaBotonAceptar,
     string observaciones,
     int userId)
        {
            string SQLInsert = @"
        INSERT INTO LlamadosProspectos
        (IdProspecto, CodRespuesta, Fecha, HoraInicio, HoraFin, ObsProspecto, UserId)
        VALUES
        (@IdProspecto, @CodRespuesta, @Fecha, @HoraInicio, @HoraFin, @ObsProspecto, @UserId)
    ";

            Dictionary<string, object> parametros = new Dictionary<string, object>
    {
        { "@IdProspecto", idProspectoActual },
        { "@CodRespuesta", codRespuesta },
        { "@Fecha", fechaBotonAceptar.ToString("yyyy-MM-dd") },
        { "@HoraInicio", horaBotonAceptar.ToString("HH:mm:ss") },
        { "@HoraFin", DateTime.Now.ToString("HH:mm:ss") },
        { "@ObsProspecto", observaciones },
        { "@UserId", userId }
    };

            try
            {
                int rowsAffected = operaciones.Ejecutar(SQLInsert, parametros);

                servicioMensajes.ManejarResultado(
                    rowsAffected > 0,
                    rowsAffected > 0 ?
                        "Se insertó correctamente en LlamadosProspectos." :
                        "Error al insertar."
                );
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
            }
        }



        //llenar la tabla Agendados 
        public void LlenarAgendados(int idProspectoActual, DateTime fechaAgendado, TimeSpan horaAgenda, string obsProspecto, int userId)
        {
            string SQLInsert = @"
        INSERT INTO Agendados 
        (IdProspecto, FechaAgendado, HoraAgenda, ObsProspecto, UserId) 
        VALUES 
        (@IdProspecto, @FechaAgendado, @HoraAgenda, @ObsProspecto, @UserId)";

            Dictionary<string, object> parametros = new Dictionary<string, object>
    {
        { "@IdProspecto", idProspectoActual },
        { "@FechaAgendado", fechaAgendado.Date },
        { "@HoraAgenda", horaAgenda },
        { "@ObsProspecto", obsProspecto },
        { "@UserId", userId }
    };

            try
            {
                int rowsAffected = operaciones.Ejecutar(SQLInsert, parametros);

                servicioMensajes.ManejarResultado(
                    rowsAffected > 0,
                    rowsAffected > 0 ?
                        "Se insertó correctamente el registro en la tabla Agendados." :
                        "Error al insertar el registro en la tabla Agendados."
                );
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
            }
        }




        public DataTable ListarLosLlamadosAdmin()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
            string query = @"SELECT P.Apellido, P.Nombre, P.TelefonoPrincipal, R.Respuesta AS Respuestas, LP.Fecha, LP.Observacion, LP.CodRespuesta, LP.UserId
                             FROM Prospectos P
                             INNER JOIN LlamadosProspectos LP ON P.IdProspecto = LP.IdProspecto
                             INNER JOIN Respuestas R ON LP.CodRespuesta = R.CodRespuesta";

            DataTable dataTable = new DataTable();

            try
            {
                dataTable = operaciones.Seleccion(query, null);
                servicioMensajes.ManejarResultado(true, "Se listaron correctamente los llamados.");
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
                // En caso de error, retornamos un DataTable vacío para evitar referencia nula
                return new DataTable();
            }

            return dataTable;
        }
        public DataTable ListarLosLlamados(int userId)
        {
            string SQLSelect = @"
            SELECT 
                P.Apellido, 
                P.Nombre, 
                P.TelefonoPrincipal, 
                R.Respuesta AS Respuestas, 
                LP.Fecha, 
                LP.ObsProspecto, 
                LP.CodRespuesta
            FROM 
                Prospectos P
            INNER JOIN 
                LlamadosProspectos LP ON P.IdProspecto = LP.IdProspecto
            INNER JOIN 
                Respuestas R ON LP.CodRespuesta = R.CodRespuesta
            WHERE 
                LP.UserId = @UserId";

            // SIEMPRE inicializamos un DataTable vacío
            DataTable dataTable = new DataTable();

            try
            {
                Dictionary<string, string> parametros = new Dictionary<string, string>
        {
            { "@UserId", userId.ToString() }
        };

                var resultado = operaciones.Seleccion(SQLSelect, parametros);

                // Si la consulta devolvió null ⇒ devolvemos el DataTable vacío
                if (resultado == null)
                {
                    servicioMensajes.ManejarResultado(false, "No se encontraron registros.");
                    return dataTable;
                }

                dataTable = resultado;

                servicioMensajes.ManejarResultado(true, "Se listaron correctamente los llamados.");
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
                return dataTable; // devolvemos vacío, no null
            }

            return dataTable;
        }



        //Consultar Agendados
        public DataTable ListarAgendadosAdmin()
        {
            string query = @"
        SELECT 
            P.IdProspecto,
            P.Nombre,
            P.Apellido,
            P.TelefonoPrincipal,
            A.FechaAgendado,
            A.HoraAgenda,
            A.ObsProspecto,
            A.UserId
        FROM Prospectos P
        INNER JOIN Agendados A ON P.IdProspecto = A.IdProspecto";

            try
            {
                return operaciones.Seleccion(query, null) ?? new DataTable();
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
                return new DataTable();
            }
        }

        //Consultar Agendados
        public DataTable ListarAgendados(int userId)
        {
            string SQLSelect = @"
                SELECT 
                    P.IdProspecto, 
                    P.Nombre, 
                    P.Apellido, 
                    P.TelefonoPrincipal, 
                    A.FechaAgendado, 
                    A.HoraAgenda, 
                    A.ObsProspecto
                FROM 
                    Prospectos P
                INNER JOIN 
                    Agendados A ON P.IdProspecto = A.IdProspecto
                WHERE 
                    A.UserId = @UserId"; // Asumiendo que hay una columna UserId en la tabla Agendados

            DataTable dataTable = null!;

            try
            {
                Dictionary<string, string> parametros = new Dictionary<string, string>
                {
                    { "@UserId", userId.ToString() }
                };

                dataTable = operaciones.Seleccion(SQLSelect, parametros);
                servicioMensajes.ManejarResultado(true, "Se listaron correctamente los agendados.");
            }
            catch (Exception ex)
            {
                servicioMensajes.ManejarExcepcion(ex);
            }

            return dataTable ?? new DataTable(); // Nos aseguramos que devuelva un DataTable aunque esté vacío
        }
        public void EliminarAgendado(int idProspecto)
        {
            string SQLDelete = "DELETE FROM Agendados WHERE IdProspecto = @IdProspecto";
            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@IdProspecto", idProspecto.ToString() }
            };
            operaciones.Ejecutar(SQLDelete, parametros);
        }

    }
}
