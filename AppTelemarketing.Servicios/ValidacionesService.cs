using AppTelemarketing.CapaDatos;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace AppTelemarketing.Servicios
{
    public class ValidacionesService
    {
        //ValidacionesService validarMensaje = new ValidacionesService();
        private OperacionesBD operaciones = new OperacionesBD();


        public ManejoMensajesServices.ResultadoAlta ActualizarContraseña(string nombredeUsuario, string nuevaContraseña)
        {
            var resultado = new ManejoMensajesServices.ResultadoAlta();



            string SQLUpdate = "UPDATE Usuarios SET Clave = @nuevaContraseña WHERE UserName = @nombredeUsuario";

            Dictionary<string, object> parametros = new Dictionary<string, object>
            {
                { "@nuevaContraseña", nuevaContraseña },
                { "@nombredeUsuario", nombredeUsuario }
            };


            try
            {
                int filasAfectadas = operaciones.Ejecutar(SQLUpdate, parametros);

                if (filasAfectadas > 0)
                {
                    resultado.Exito = true;
                    resultado.MensajeExitoOError = "La contraseña ha sido actualizada con éxito.";
                }
                else
                {
                    resultado.Exito = false;
                    resultado.MensajeExitoOError = "No se pudo actualizar la contraseña.";
                }
            }
            catch (SqlException sqlEx)
            {
                resultado.Exito = false;
                resultado.MensajeExitoOError = $"Error SQL: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                resultado.Exito = false;
                resultado.MensajeExitoOError = $"Error : {ex.Message}";
            }

            return resultado;
        }


        //PREGUNTAR AL PROFE COMO VALIDAR EL EMAIL
        public bool IsValidEmail(string email)
        {
            // Expresión regular para validar el formato de correo electrónico
            Regex expr = new Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            return expr.IsMatch(email);
        }

        //validacion de formato de telefono
        public bool EsTelefonoValido(string telefono, out string mensajeError, out long telefonoPrincipal)
        {
            bool conversionTelefonoExitosa = false;
            telefonoPrincipal = 0;
            mensajeError = string.Empty;

            // Verificamos si el campo está vacío
            if (string.IsNullOrWhiteSpace(telefono))
            {
                mensajeError = "El campo de teléfono no puede estar vacío.";
                return false;
            }

            // Verificamos que el número de teléfono no exceda los 10 dígitos
            if (telefono.Length > 10)
            {
                mensajeError = "El número de teléfono no puede tener más de 10 dígitos\n\nNi Caracteres no válidos(-,*,/,etc)";
                return false;
            }

            // Intento de  convertir el texto del teléfono a entero
            conversionTelefonoExitosa = long.TryParse(telefono, out telefonoPrincipal);

            if (conversionTelefonoExitosa)
            {
                // Verificar que el teléfono no sea cero
                if (telefonoPrincipal == 0)
                {
                    mensajeError = "El teléfono no puede ser cero.";
                    return false;
                }

                // Verificar que el teléfono tiene entre 8 y 10 dígitos
                if (telefono.Length != 8 && telefono.Length != 10)
                {
                    mensajeError = "La cantidad de dígitos del teléfono es incorrecta.\n\n(La cantidad de dígitos debe ser 8 o 10)";
                    return false;
                }
            }
            else
            {
                mensajeError = "El teléfono ingresado tiene caracteres no válidos(-,*,/,etc).";
                return false;
            }

            // Si todas las validaciones pasan
            return true;
        }

    }
}
