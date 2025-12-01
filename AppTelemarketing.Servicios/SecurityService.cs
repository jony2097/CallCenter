using AppTelemarketing.CapaDatos;
using System.Configuration;

namespace AppTelemarketing.Servicios
{
    public class SecurityService
    {
        OperacionesBD operaciones = new OperacionesBD();

        public bool UsuarioValido(string username, string clave, int tipoUsuario)
        {
            bool valido = false;

            string SQLSelect = @"
                SELECT * 
                FROM Usuarios 
                WHERE UserName = @username
                AND Clave = @clave
                AND IdTipoUsuario = @tipoUsuario
                AND (
                        (@tipoUsuario = 1)              -- ADMIN entra siempre
                        OR (AceptadoXAdmin = 1)         -- TMK debe estar aprobado
                    )";

            // 🔥 IMPORTANTE → todo string (tu clase Seleccion lo exige)
            var parametros = new Dictionary<string, string>
            {
                { "@username", username },
                { "@clave", clave },
                { "@tipoUsuario", tipoUsuario.ToString() }
            };

            try
            {
                var resultado = operaciones.Seleccion(SQLSelect, parametros);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    valido = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }

            return valido;
        }

        // Método usado en cambiar contraseña
        public bool UsuarioValido(string username, string clave)
        {
            bool valido = false;
            string SQLSelect = "SELECT * FROM Usuarios WHERE UserName=@username AND Clave=@clave";

            var parametros = new Dictionary<string, string>
            {
                { "@username", username },
                { "@clave", clave }
            };

            try
            {
                var resultado = operaciones.Seleccion(SQLSelect, parametros);
                if (resultado != null && resultado.Rows.Count > 0)
                {
                    valido = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }

            return valido;
        }
    }
}
