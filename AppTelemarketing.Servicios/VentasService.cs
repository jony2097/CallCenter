using AppTelemarketing.CapaDatos;
using AppTelemarketing.Entidades;
using System;
using System.Collections.Generic;
using System.Data;

namespace AppTelemarketing.Servicios
{
    public class VentasService
    {
        OperacionesBD operaciones = new OperacionesBD();

        // =============================================================
        //  CARGAR VENTA - AHORA DEVUELVE EL ERROR AL FORM
        // =============================================================
        public (bool exito, string mensajeError) CargarVenta(Venta venta)
        {
            string SQLInsert = @"INSERT INTO Ventas 
            (IdProspecto, TelSecundario, Direccion, IdLocalidad, IdProvincia, CP, Fecha, CodProducto, Total, Mail, IdMedioDePago, UserId, ObservacioneVenta)
            VALUES 
            (@IdProspecto, @TelSecundario, @Direccion, @IdLocalidad, @IdProvincia, @CP, @Fecha, @CodProducto, @Total, @Mail, @IdMedioDePago, @UserId, @ObservacioneVenta)";

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@IdProspecto", venta.IdProspecto },
                { "@TelSecundario", venta.TelSecundario },
                { "@Direccion", venta.Direccion },
                { "@IdLocalidad", venta.IdLocalidad },
                { "@IdProvincia", venta.IdProvincia },
                { "@CP", venta.CP },
                { "@Fecha", venta.Fecha },
                { "@CodProducto", venta.CodProducto },
                { "@Total", venta.Total },
                { "@Mail", venta.Mail },
                { "@IdMedioDePago", venta.IdMedioDePago },
                { "@UserId", venta.UserId },
                { "@ObservacioneVenta", venta.ObservacioneVenta }
            };

            try
            {
                int filas = operaciones.Ejecutar(SQLInsert, parametros);

                if (filas > 0)
                    return (true, "");

                return (false, "No se insertó ninguna fila.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // =============================================================
        //   OBTENER VENTAS DEL USUARIO
        // =============================================================
        public DataTable ObtenerVentas(int userId)
        {
            string SQLSelect =
                @"SELECT 
                    v.NroCliente,
                    v.Fecha,
                    v.ObservacioneVenta,
                    p.Nombre,
                    p.Apellido,
                    u.UserId,
                    m.Descripcion,
                    prod.NombreProducto,
                    prod.Precio
                FROM Ventas v
                INNER JOIN Prospectos p ON v.IdProspecto = p.IdProspecto
                INNER JOIN Usuarios u ON v.UserId = u.UserId
                INNER JOIN MedioDePago m ON v.IdMedioDePago = m.IdMedioDePago
                INNER JOIN Productos prod ON v.CodProducto = prod.CodProd
                WHERE v.UserId = @UserId";

            Dictionary<string, string> parametros = new Dictionary<string, string>
            {
                { "@UserId", userId.ToString() }
            };

            try
            {
                return operaciones.Seleccion(SQLSelect, parametros);
            }
            catch
            {
                return new DataTable();
            }
        }

        // =============================================================
        //   OBTENER TODAS LAS VENTAS (ADMIN)
        // =============================================================
        public DataTable ObtenerVentasAdmin()
        {
            string query = @"
                SELECT 
                    v.NroCliente,
                    v.Fecha,
                    v.ObservacioneVenta,
                    p.Nombre,
                    p.Apellido,
                    u.UserId,
                    m.Descripcion,
                    prod.NombreProducto,
                    prod.Precio
                FROM Ventas v
                INNER JOIN Prospectos p ON v.IdProspecto = p.IdProspecto
                INNER JOIN Usuarios u ON v.UserId = u.UserId
                INNER JOIN MedioDePago m ON v.IdMedioDePago = m.IdMedioDePago
                INNER JOIN Productos prod ON v.CodProducto = prod.CodProd
            ";

            try
            {
                return operaciones.Seleccion(query, null);
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}
