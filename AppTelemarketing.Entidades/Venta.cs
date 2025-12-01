using System;

namespace AppTelemarketing.Entidades
{
    public class Venta
    {
        public int IdProspecto { get; set; }
        public int TelSecundario { get; set; }
        public string Direccion { get; set; } = "";
        public int IdLocalidad { get; set; }
        public int IdProvincia { get; set; }
        public int CP { get; set; }
        public DateTime Fecha { get; set; }
        public TimeOnly HoraComienzo { get; set; }
        public TimeOnly HoraFin { get; set; }
        public int CodProducto { get; set; }
        public decimal Total { get; set; }
        public string Mail { get; set; } = "";
        public int IdMedioDePago { get; set; }
        public int UserId { get; set; }
        public string ObservacioneVenta { get; set; } = "";

        public Venta(
            int idProspecto,
            int telSecundario,
            string direccion,
            int idLocalidad,
            int idProvincia,
            int cp,
            DateTime fecha,
            TimeOnly horaComienzo,
            TimeOnly horaFin,
            int codProducto,
            decimal total,
            string mail,
            int idMedioDePago,
            int userId,
            string observacioneVenta)
        {
            IdProspecto = idProspecto;
            TelSecundario = telSecundario;
            Direccion = direccion;
            IdLocalidad = idLocalidad;
            IdProvincia = idProvincia;
            CP = cp;
            Fecha = fecha;
            HoraComienzo = horaComienzo;
            HoraFin = horaFin;
            CodProducto = codProducto;
            Total = total;
            Mail = mail;
            IdMedioDePago = idMedioDePago;
            UserId = userId;
            ObservacioneVenta = observacioneVenta;
        }

        public Venta() { }
    }
}
