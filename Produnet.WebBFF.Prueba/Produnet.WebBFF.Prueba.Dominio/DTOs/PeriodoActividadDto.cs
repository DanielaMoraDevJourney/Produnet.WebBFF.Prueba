using System;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.DTOs
{
    public class PeriodoActividadDto
    {
        public Guid Id { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
    }
}
