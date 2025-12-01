using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTelemarketing.Entidades
{
    public class Usuario
    {
        public int UserId { get; }
        public string UserName { get; set; }
        public string Nombre { set; get; } //Propiedad automáticamente implemetada
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public int TipoUsuario { get; set; }

        public int AceptadoXAdmin { get; set; }

        public Usuario(string userName, string nombre, string apellido, string correo, string clave, int tipoUsuario, int aceptadoXAdmin)
        {

            UserName = userName;
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Clave = clave;
            TipoUsuario = tipoUsuario;
            AceptadoXAdmin = aceptadoXAdmin;
        }

        // Constructor para inicializar cuando userId es necesario
        public Usuario(int userId, string userName, string nombre, string apellido, string correo, string clave, int tipoUsuario, int aceptadoXAdmin)
        {
            UserId = userId;
            UserName = userName;
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            Clave = clave;
            TipoUsuario = tipoUsuario;
            AceptadoXAdmin = aceptadoXAdmin;
        }

        public Usuario(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
            
        }
        public Usuario( int aceptadoXAdmin)
        {
            AceptadoXAdmin= aceptadoXAdmin;

        }
    }

}
