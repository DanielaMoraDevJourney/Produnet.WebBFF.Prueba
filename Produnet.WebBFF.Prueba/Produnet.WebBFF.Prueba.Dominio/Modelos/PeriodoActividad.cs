using System;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio
{
    public class PeriodoActividad
    {
        public Guid Id { get; set; }
        public Guid SesionUsuarioId { get; set; }
        public SesionUsuario SesionUsuario { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
    }
}
