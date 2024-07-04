using System;
using System.Collections.Generic;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.DTOs
{
    public class SesionUsuarioDto
    {
        public Guid Id { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
        public ICollection<PeriodoInactividadDto> PeriodosInactividad { get; set; } = new List<PeriodoInactividadDto>();
        public ICollection<PeriodoActividadDto> PeriodosActividad { get; set; } = new List<PeriodoActividadDto>();
        public TimeSpan TotalInactividad { get; set; }
        public TimeSpan TotalActividad { get; set; }
    }
}
