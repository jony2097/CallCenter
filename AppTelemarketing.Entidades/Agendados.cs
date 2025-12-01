using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTelemarketing.Entidades
{
    public class Agendados
    {

        public int IdAgendado { get;  }
        public int IdProspecto { get; set; }
        public DateTime FechaAgendado { get; set; }
        public TimeSpan HoraAgenda { get; set; }
        public string? ObsProspecto { get; set; }

       

        // Constructor con parámetros
        public Agendados(int idProspecto, DateTime fechaAgendado, TimeSpan horaAgenda, string? obsProspecto = null)
        {
            IdProspecto = idProspecto;
            FechaAgendado = fechaAgendado;
            HoraAgenda = horaAgenda;
            ObsProspecto = obsProspecto;
        }

       
    }
}
