using System;
using System.Collections.Generic;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio
{
    public class SesionUsuario
    {
        public Guid Id { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public ICollection<PeriodoInactividad> PeriodosInactividad { get; set; } = new List<PeriodoInactividad>();
        public ICollection<PeriodoActividad> PeriodosActividad { get; set; } = new List<PeriodoActividad>();
    }
}
