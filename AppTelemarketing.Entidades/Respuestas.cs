using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTelemarketing.Entidades
{
    public class Respuestas
    {
      public int CodRespuesta { get; set; }
      public string Respuesta { get; set; }

      // Constructor con parámetros
      public Respuestas(int codRespuesta, string respuesta)
        {
            CodRespuesta = codRespuesta;
            Respuesta = respuesta;
        }

       public  Respuestas(string respuesta) {
            Respuesta = respuesta;
        }
    }
}
