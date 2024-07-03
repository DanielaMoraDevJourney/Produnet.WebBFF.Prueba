namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio
{
        public class PeriodoInactividad
    {
        public Guid Id { get; set; }
        public Guid SesionUsuarioId { get; set; }
        public SesionUsuario SesionUsuario { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime? HoraFin { get; set; }
    }
}
