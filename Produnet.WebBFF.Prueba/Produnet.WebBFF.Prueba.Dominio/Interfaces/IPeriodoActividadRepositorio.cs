using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces
{
    public interface IPeriodoActividadRepositorio
    {
        Task<PeriodoActividad> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<PeriodoActividad>> ObtenerTodasAsync();
        Task AgregarAsync(PeriodoActividad periodoActividad);
        Task ActualizarAsync(PeriodoActividad periodoActividad);
        Task EliminarAsync(Guid id);
    }
}
